$(document).ready(function () {
    $("#makePaymentButton").on("click", function (event) {
        console.log("Starting AJAX call for payment");
        event.preventDefault();

        var amount = $("#preFilledAmount").val();
        var transactionId = $("#transactionId").val();
        var userId = $("#userId").val();
        console.log("Amount: " + amount + " TransactionId: " + transactionId + " userId: " + userId);

        $.ajax({
            url: "/BankPayment/ProcessPayment",
            type: "POST",
            data: { amount: amount, transactionId: transactionId, userId: userId },
            success: function (result) {
                console.log("AJAX success:", result);
                if (result.success) {
                    console.log(result.message);
                    alert(result.message);
                    window.location.href = "https://localhost:44398/";
                } else {
                    console.error(result.message);
                    alert("Error: " + result.message);
                }
            },
            error: function (error) {
                console.error("AJAX error:", error);
                alert("An error occurred while processing your request.");
            }
        });
    });
});
