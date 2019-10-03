var userObj = localStorage.getItem('user');

$(document).ready(function () {
    if (userObj) {
        getPendingsOrders();
    }
    else {
        window.location.href = window.location.protocol + '//' + window.location.host + '/Zitro/Index'
    }
});

//Method to Get Pedding Orders
function getPendingsOrders() {
    $.ajax({
        url: '/Orders/getPendingOrders',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            getZitroPendingOrders(data.orders);
        },
        complete: function () {
            console.log('complete')
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}

//Method to filter the pending orders with a gateway Zitro and financial status Pending, and print in a table the orders.
function getZitroPendingOrders(orders) {
    var items = '';
    var table = $('#dtBasicExample').DataTable();
    if (orders) {
        var zitroPendingOrders = orders.filter(function (data) { return data.gateway === 'Zitro' && data.financial_status === 'pending'; })
        for (var i = 0; i < zitroPendingOrders.length; i++) {
            var name = zitroPendingOrders[i].name.replace("#", "");
            items += '<tr id="' + zitroPendingOrders[i].id + '"><td>' + zitroPendingOrders[i].customer.first_name + ' ' + zitroPendingOrders[i].customer.last_name + '</td ><td>' + zitroPendingOrders[i].name + '</td><td>' + zitroPendingOrders[i].total_price + '</td><td><button onclick="markAsPaid(' + zitroPendingOrders[i].id + ',' + zitroPendingOrders[i].name.replace("#", "") + ')">Mark as paid</button></td></tr>';
        }
    }
    $("#tableOrders").append(items)
}


//Method to mark as paid an order
function markAsPaid(orderId, name) {
    if (validateFieldsGC(orderId)) {
        $.ajax({
            url: '/Home/getTransactions?id_transaction=' + orderId,
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (data) {
                console.log('Success');
                sendToPaid(data.transactions[0], name);
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


function validateFieldsGC(orderTransaction) {
    if (!orderTransaction) {
        return false;
    }
    else {
        return true
    }
}

//Method to Send a transaction to paid
function sendToPaid(transaction, name) {
    var dataToMark = buildDataToPaid(transaction);
    $.ajax({
        url: '/Home/markToPaid',
        type: 'POST',
        async: false,
        data: dataToMark,
        dataType: 'json',
        success: function (data) {
            $("#" + transaction.order_id).remove();
            sendEmail(name);
        },
        complete: function () {
            console.log('complete')
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}

//Method to build the object that we will mark as paid.
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

//Method to mark send an email to the administrator
function sendEmail(orderId) {
    $.ajax({
        url: '/Orders/sendMail?id_transaction=' + orderId ,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            console.log('Success');
        },
        complete: function () {
            console.log('complete')
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}