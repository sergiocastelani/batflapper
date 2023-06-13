
function PrepareAd()
{}

function adStatusCallback(status) {
    console.log('Ad Status: ' + status);
}

const userUUIDv4 = crypto.randomUUID();

function ShowAd()
{
    var options = {
        zoneId: 2050, //5824,
        accountId: 7479,
        userId: userUUIDv4,
        adStatusCb: adStatusCallback
    };

    invokeApplixirVideoUnit(options);
}

