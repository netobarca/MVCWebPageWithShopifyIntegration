// Getting Variables frrom URL to mark a GiftCard as Paid
var totalAmount = GetURLParameter('ta');
var transaction = GetURLParameter('tr');
var productType = GetURLParameter('pt');
var orderId = GetURLParameter('oi');;

$(document).ready(function () {
    if (productType) {
        if (productType == "GC") {
            markAsPaid();
        }
        else {
            redirectToAtlant();
        }
    }
    else {
        window.location.href = window.location.protocol + '//' + window.location.host + '/Zitro/Index'
    }
});

// Method to get a transaction to paid
function markAsPaid() {
    if (validateFieldsGC()) {
        $.ajax({
            url: '/Home/getTransactions?id_transaction=' + transaction,
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (data) {
                console.log('Success');
                sendToPaid(data.transactions[0]);
            },
            complete: function () {
                console.log('complete')
            },
            error: function () {
                console.log(JSON.stringify(request));
            }
        });
    }
}

function validateFieldsGC() {
    if (!transaction) {
        return false;
    }
    else {
        return true
    }
}

//Method to mark a transaction as paid
function sendToPaid(transaction) {
    var dataToMark = buildDataToPaid(transaction);
    $.ajax({
        url: '/Home/markToPaid',
        type: 'POST',
        async: false,
        data: dataToMark,
        dataType: 'json',
        success: function (data) {
        },
        complete: function () {
            console.log('complete')
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}

//Method to build the object that we will mark as paid
function buildDataToPaid(baseTrans) {
    var data = {
        "transaction": {
            "kind": "capture",
            "gateway": "manual",
            "amount": baseTrans.amount,
            "parent_id": baseTrans.id,
            "status": "success",
            "currency": baseTrans.currency,
            "order_id": baseTrans.order_id
        },
    };
    return data;
}


//Method to mark the parameter as paid
function GetURLParameter(sParam) {
    var sPageUrl = window.location.search.substring(1);
    var sURLVariables = sPageUrl.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split("=");
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}