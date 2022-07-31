// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var purchaseDetailsModal = document.getElementById('purchaseDetailsModal')
purchaseDetailsModal.addEventListener('show.bs.modal', function (event) {
    // Button that triggered the modal
    var button = event.relatedTarget
    // Extract info from data-bs-* attributes
    var purchaseDetails = button.getAttribute('data-bs-details')
    var userId = button.getAttribute('data-bs-user')
    // If necessary, you could initiate an AJAX request here
    // and then do the updating in a callback.
    //
    // Update the modal's content.
    var purchaseDate = exampleModal.querySelector('.purchase-date')
    var purchasePrice = exampleModal.querySelector('.purchase-price')
    var purchaseNo = exampleModal.querySelector('.purchase-number')

    purchaseDate.textContent = purchaseDetails.PurchaseDateTime

})