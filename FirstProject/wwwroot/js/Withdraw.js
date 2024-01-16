$(document).ready(function () {
    $("#withdrawButton").unbind("click").click(function (event) {
        console.log("starting ajax call for withdrawal");
        event.preventDefault();
        var amount = $("#withdrawAmount").val();

        $.ajax({
            url: "/Withdraw/MakeWithdrawal",
            type: "POST",
            data: { amount: amount },
            contentType: "application/x-www-form-urlencoded; charset=UTF-8", 
            success: function (result) {
                if (result.success) {
                    console.log(result.message);
                    alert(result.message);
                    console.log("aqamde movida");
                    window.location.href = "/Home";
                } else {
                    console.error(result.message);
                    alert("Error: " + result.message);
                    console.log("aqamde movida mara elsia");
                }
            },
            error: function (error) {
                console.error("Withdrawal failed", error);
                alert("An error occurred while processing your request.");
            }
        });
    });
});
