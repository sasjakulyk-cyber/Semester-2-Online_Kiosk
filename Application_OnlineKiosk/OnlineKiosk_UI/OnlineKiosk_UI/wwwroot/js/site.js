// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectVariant(variantId) {
    $(".variant-card").removeClass("active");
    $(".add-detail-cart .price").html($('#variant-' + variantId + " .variant-price").html());
    $('#variant-' + variantId).addClass("active");
}