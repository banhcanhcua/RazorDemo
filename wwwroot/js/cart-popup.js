// Hiển thị popup thông báo thành công khi thêm vào giỏ hàng
function showCartSuccess(message) {
    let popup = document.createElement('div');
    popup.className = 'cart-success-popup';
    popup.innerHTML = `<span>${message}</span>`;
    document.body.appendChild(popup);
    setTimeout(() => {
        popup.classList.add('show');
    }, 10);
    setTimeout(() => {
        popup.classList.remove('show');
        setTimeout(() => document.body.removeChild(popup), 400);
    }, 2000);
}
