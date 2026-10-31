// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectVariant(variantId) {
    $(".variant-card").removeClass("selected");
    $(".add-detail-cart .price").html($('#variant-' + variantId + " .variant-price").html());
    $('#variant-' + variantId).addClass("selected");
    $(".detail-actions input[name='variantId']").val(variantId);
}

function increaseCount()
{
    var currentValue = parseInt($(".detail-actions input[name='quantity']").val(), 10);
    currentValue += 1;
    if (currentValue > 99) {
        currentValue = 99;
    }
    $(".detail-actions .quantity").html(currentValue);
    $(".detail-actions input[name='quantity']").val(currentValue)

}

$(function () {
    $(".quantity-form").on("submit", function (event) {
        event.preventDefault();

        const $form = $(this);
        const $button = $form.find("button[type='submit']");
        $button.prop("disabled", true);

        $.ajax({
            url: $form.attr("action"),
            type: "POST",
            data: $form.serialize(),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
            .done(function (result) {
                const $cartItem = $(`[data-cart-item='${result.variantId}']`);

                if (result.removed) {
                    $cartItem.remove();
                } else {
                    $cartItem.find(".cart-item-quantity").text(result.quantity);
                    $cartItem.find(".cart-item-subtotal").text(formatCurrency(result.itemSubtotal));
                    $cartItem.find(".quantity-increase-button")
                        .prop("disabled", result.quantity >= 99);
                }

                $(".cart-subtotal-value").text(formatCurrency(result.subtotal));
                $(".cart-total-value").text(formatCurrency(result.total));

                if (result.isEmpty) {
                    $(".cart-products").html("<p>Your cart is empty.</p>");
                }

                $(".cart-confirm-btn").prop("disabled", result.isEmpty);
            })
            .fail(function () {
                window.location.reload();
            })
            .always(function () {
                if ($.contains(document, $button[0])) {
                    $button.prop("disabled", false);
                }
            });
    });
});

function formatCurrency(value) {
    return new Intl.NumberFormat(undefined, {
        style: "currency",
        currency: "EUR"
    }).format(value);
}

function decreaseCount()
{
    var currentValue = parseInt($(".detail-actions input[name='quantity']").val(), 10);
    currentValue -= 1;
    if (currentValue < 1) {
        currentValue = 1;
    }
    $(".detail-actions .quantity").html(currentValue);
    $(".detail-actions input[name='quantity']").val(currentValue)
}