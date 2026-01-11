$(document).ready(function () {

    let selectedProduct = null;

    $(".title-view.mode").click(function () {

        selectedProduct = $(this).closest(".product-block");
        const initialTourname = selectedProduct.find(".title-view").text();
        const input = selectedProduct.find(".edit");

        input.val(initialTourname);

        selectedProduct.find(".mode").toggleClass("hidden");
        selectedProduct.find(".cancel-button").removeClass("hidden")
        input.focus();

    });

    $(".cancel-button").click(function () {

        const block = $(this).closest(".product-block");

        block.find(".title-view").removeClass("hidden");
        block.find(".edit").addClass("hidden");
        block.find(".cancel-button").addClass("hidden");
        selectedProduct = null
    })

    $(".edit.mode").on("keyup", function (event) {
        // keyCode == 13 for Enter key
        if (event.keyCode == 13) {

            selectedProduct = $(this).closest(".product-block");
            const newTitle = $(this).val();

            selectedProduct.find(".title-view").text(newTitle);

            const id = selectedProduct.attr('data-id');
            const url = `${baseUrl}/Tourism/UpdateTourName?id=${id}&name=${newTitle}`;

            $.get(url);

            selectedProduct.find(".mode").toggleClass("hidden");
        }
    });

})