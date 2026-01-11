$(document).ready(function () {

    $(".buy-button").click(function () {
        const url = `${baseUrl}/Tourism/GetCountShopCartItems`
        $.get(url, function (data) {
            document.getElementById('ShopCartCount').innerText = data.countShopCartItems;
        });
    });

    function updateCartCount() {
        $.get('/Tourism/GetCountShopCartItems', function (data) {
            $("#ShopCartCount").text(data.countShopCartItems);
        });
    }
    updateCartCount()

})