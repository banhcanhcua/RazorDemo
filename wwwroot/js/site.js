// Xóa ảnh sản phẩm bằng AJAX trên trang chi tiết sản phẩm
document.addEventListener('DOMContentLoaded', function () {
	const imagesPanel = document.getElementById('productImagesPanel');
	const deleteAllBtn = document.getElementById('deleteAllImagesBtn');
	if (!imagesPanel) return;
	const productIdHidden = document.getElementById('productIdHidden');
	const productId = productIdHidden ? productIdHidden.value : null;
	function reloadImages() {
		// Reload lại phần ảnh bằng cách reload trang (hoặc có thể fetch partial nếu muốn tối ưu hơn)
		location.reload();
	}
	imagesPanel.addEventListener('click', function (e) {
		if (e.target.classList.contains('delete-image-btn')) {
			const imageUrl = e.target.getAttribute('data-image-url');
			if (confirm('Bạn có chắc muốn xóa ảnh này?')) {
				fetch('/ProductImage/DeleteImage', {
					method: 'POST',
					headers: { 'Content-Type': 'application/json' },
					body: JSON.stringify({ productId: productId, imageUrl: imageUrl })
				})
				.then(res => res.json())
				.then(data => {
					if (data.success) reloadImages();
					else alert('Xóa ảnh thất bại!');
				});
			}
		}
	});
	if (deleteAllBtn) {
		deleteAllBtn.addEventListener('click', function () {
			if (confirm('Bạn có chắc muốn xóa tất cả ảnh?')) {
				fetch('/ProductImage/DeleteAllImages', {
					method: 'POST',
					headers: { 'Content-Type': 'application/json' },
					body: JSON.stringify({ productId: productId })
				})
				.then(res => res.json())
				.then(data => {
					if (data.success) reloadImages();
					else alert('Xóa ảnh thất bại!');
				});
			}
		});
	}
});
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// AJAX update for product list (sort/search)
document.addEventListener('DOMContentLoaded', function () {
	const sortOrder = document.getElementById('sortOrder');
	const searchKeyword = document.getElementById('searchKeyword');
	const productPanel = document.querySelector('.content-panel');
	if (!sortOrder || !searchKeyword || !productPanel) return;

	function updateProductList(page = 1) {
		const sort = sortOrder.value;
		const keyword = searchKeyword.value;
		fetch(`/ProductPartial/List?sortOrder=${sort}&page=${page}&pageSize=15&keyword=${encodeURIComponent(keyword)}`)
			.then(res => res.text())
			.then(html => {
				productPanel.innerHTML = html;
				attachPaginationEvents();
			});
	}

	sortOrder.addEventListener('change', function (e) {
		updateProductList(1);
	});
	searchKeyword.addEventListener('input', function (e) {
		updateProductList(1);
	});

	function attachPaginationEvents() {
		productPanel.querySelectorAll('.pagination .page-link').forEach(link => {
			link.addEventListener('click', function (e) {
				e.preventDefault();
				const url = new URL(link.href);
				const page = url.searchParams.get('page') || 1;
				updateProductList(page);
			});
		});
	}
	attachPaginationEvents();
});
