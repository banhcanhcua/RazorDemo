function showStatusPopup(message, status = 'success') {
	const popup = document.createElement('div');
	popup.className = `status-popup ${status === 'error' ? 'status-popup-error' : 'status-popup-success'}`;
	popup.innerHTML = `<span>${message}</span>`;
	document.body.appendChild(popup);
	setTimeout(() => {
		popup.classList.add('show');
	}, 10);
	setTimeout(() => {
		popup.classList.remove('show');
		setTimeout(() => document.body.removeChild(popup), 400);
	}, 2200);
}

document.addEventListener('DOMContentLoaded', function () {
	const pageStatusPopup = document.getElementById('pageStatusPopup');
	if (!pageStatusPopup) return;

	const message = pageStatusPopup.dataset.message;
	const status = pageStatusPopup.dataset.status || 'success';
	if (message) {
		showStatusPopup(message, status);
	}
});

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
	const filterForm = document.getElementById('productFilterForm');
	const clearFiltersButton = document.getElementById('clearFiltersButton');
	const sortOrder = document.getElementById('sortOrder');
	const searchKeyword = document.getElementById('searchKeyword');
	const productPanel = document.querySelector('.content-panel');
	if (!filterForm || !clearFiltersButton || !sortOrder || !searchKeyword || !productPanel) return;

	function buildQuery(page = 1) {
		const params = new URLSearchParams({
			sortOrder: sortOrder.value,
			page: page.toString(),
			pageSize: '15',
			keyword: searchKeyword.value
		});

		return params.toString();
	}

	function scrollToProductList() {
		const targetTop = productPanel.getBoundingClientRect().top + window.scrollY - 110;
		window.scrollTo({
			top: Math.max(targetTop, 0),
			behavior: 'smooth'
		});
	}

	function updateProductList(page = 1, options = {}) {
		const { scrollToList = false } = options;
		const query = buildQuery(page);
		fetch(`/ProductPartial/List?${query}`)
			.then(res => res.text())
			.then(html => {
				productPanel.innerHTML = html;
				history.replaceState(null, '', query ? `/?${query}${scrollToList ? '#product-list' : ''}` : '/');
				attachPaginationEvents();
				if (scrollToList) {
					scrollToProductList();
				}
			});
	}

	filterForm.addEventListener('submit', function (e) {
		e.preventDefault();
		updateProductList(1);
	});

	clearFiltersButton.addEventListener('click', function () {
		sortOrder.value = 'asc';
		searchKeyword.value = '';
		updateProductList(1);
	});

	sortOrder.addEventListener('change', function (e) {
		updateProductList(1);
	});

	function attachPaginationEvents() {
		productPanel.querySelectorAll('.pagination .page-link').forEach(link => {
			link.addEventListener('click', function (e) {
				e.preventDefault();
				const url = new URL(link.href);
				const page = url.searchParams.get('page') || 1;
				updateProductList(page, { scrollToList: true });
			});
		});
	}
	attachPaginationEvents();
});
