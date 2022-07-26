// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('a[rel=popover]').popover({
    html: true,
    trigger: 'hover',
    placement: 'right',
    content: function () { return '<img src="' + $(this).data('img') + '" />'; }
});