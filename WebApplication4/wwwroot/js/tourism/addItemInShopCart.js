$(document).ready(function () {

    $(".buy-button").click(function () {
        const tourId = $(this).data("tourid");
        const url = `${baseUrl}/Tourism/AddTourInShopCart/`
        $.post(url, { tourId: tourId }, function(){
         updateCartCount();
        });
       
    });


    function updateCartCount() {
        $.get('/Tourism/GetCountShopCartItems', function (data) {
            $("#ShopCartCount").text(data.countShopCartItems);
        });
    }
    updateCartCount()
})