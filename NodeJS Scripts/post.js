const Request = require("request");
const Util = require("util");

const url = "http://localhost:56260/api/sale";

Request.post({
    "headers": { "content-type": "application/json" },
    "url": url,
    "body": JSON.stringify({
        "SaleDetail": [
            { "Quantity": 1, "Product": { "Id": 2 } },
            { "Quantity": 1, "Product": { "Id": 3 } }
        ]
    })
}, (error, response, body) => {
    if (error) {
        console.log(error);
    }
    else if (body) {
        // Se imprime el JSON de la venta que se acaba de insertar
        console.log(Util.inspect(JSON.parse(body), false, null, true));
    }
});