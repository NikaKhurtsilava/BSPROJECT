function updateBalance() {
    console.log("Updating balance...");

    
    $.ajax({
        url: '/Wallet/GetCurrentBalance',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Balance update successful:", data);
            
            $('#current-balance').text('' + data.currentBalance.toFixed(2));
        },
        error: function (error) {
            console.error('Error updating balance:', error);
            
        }
    });
}

$(document).ready(function () {
    updateBalance();

    setInterval(updateBalance, 30000); 
});