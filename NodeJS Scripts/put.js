const Request = require("request");
const Util = require("util");

const id = 86;
const quantity = 2;
const url = `http://localhost:56260/api/sale?id=${id}&quantity=${quantity}`;

Request.put({
    "headers": { "content-type": "application/json" },
    "url": url//,
    /*"body": JSON.stringify({
        "SaleDetail": [
            { "Quantity": 1, "Product": { "Id": 2 } },
            { "Quantity": 1, "Product": { "Id": 3 } }
        ]
    })*/
}, (error, response, body) => {
    if (error) {
        console.log(error);
    }
    else if (body) {
        // Se imprime el JSON del detalle de venta que se acaba de actualizar
        console.log(Util.inspect(JSON.parse(body), false, null, true));
    }
});