$(document).ready(function () {



    $(document).on("click", ".qty-discount-update", function () {
        const product_id = $(this).data("product-id");
        const quantity = document.getElementById(`${product_id}-quantity`).value;
        const discountPercent = document.getElementById(`${product_id}-discountPercent`).value;
        const store_id = @ViewBag.StoreId;
        console.log(quantity + discountPercent);
        $.ajax({
            url: '/Product/UpdateStoreQtyAndDiscount',
            type: 'POST',
            data: {
                storeId: store_id,
                productId: product_id,
                quantity: quantity,
                discountPercent: discountPercent

            },
            success: function (response) {
                const test = JSON.parse(response);
                const quantityElement = document.getElementById(`${product_id}-quantity`);
                const discountPercentElement = document.getElementById(`${product_id}-discountPercent`);
                quantityElement.value = test.Quantity;
                discountPercentElement.value = test.DiscountPercent;
                console.log(test);
            }
        });
    });


    document.querySelectorAll('.discount-adjust-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const input = btn.parentElement.querySelector('.discount-input');
            let value = parseInt(input.value);
            value = btn.dataset.operation === 'add' ? Math.min(value + 1, 100) : Math.max(value - 1, 0);
            input.value = value;
            updateHiddenInputs(input, 'discountPercentInput');
        });
    });

    document.querySelectorAll('.quantity-adjust-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const input = btn.parentElement.querySelector('.quantity-input');
            let value = parseInt(input.value);
            value = btn.dataset.operation === 'add' ? value + 1 : Math.max(value - 1, 0);
            input.value = value;
            updateHiddenInputs(input, 'quantityInput');
        });
    });

    document.querySelectorAll('.discount-input, .quantity-input').forEach(input => {
        input.addEventListener('input', () => updateHiddenInputs(input, input.classList.contains('discount-input') ? 'discountPercentInput' : 'quantityInput'));
    });

    function updateHiddenInputs(input, hiddenInputId) {
        document.getElementById(hiddenInputId).value = input.value;
    }

});
