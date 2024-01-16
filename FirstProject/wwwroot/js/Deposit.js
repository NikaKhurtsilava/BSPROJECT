document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("depositButton").removeEventListener("click", makeDepositHandler);
    document.getElementById("depositButton").addEventListener("click", makeDepositHandler);

    function makeDepositHandler(event) {
        console.log("starting ajax call for deposit");
        event.preventDefault();
        var amount = document.getElementById("amount").value;

        $.ajax({
            url: "/Deposit/MakeDeposit",
            type: "POST",
            data: { amount: amount },
            success: function (result) {
                if (result.success) {
                    console.log(result.message);
                    alert(result.message);
                    console.log(result.paymentUrl);
                    window.location.href = result.paymentUrl;
                } else {
                    console.error(result.message);
                    alert("Error: " + result.message);
                }
            },
            error: function (error) {
                console.error("Deposit failed", error);
                alert("An error occurred while processing your request.");
            }
        });
    }
});
