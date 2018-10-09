const Request = require("request");
const Readline = require("readline");
const Util = require("util");

const rl = Readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const baseURL = "http://localhost:56260/api/sale";

const menuChoice = () => {
    return new Promise((resolve, reject) => {
        try {
            rl.question("Choose an option\n1. Delete a sale\n2. Delete a sale detail\nOPTION: ", (choice) => {
                resolve(parseInt(choice));
            });
        } catch (error) {
            reject(new Error(error));
        }
    });
}

const setIdParam = () => {
    return new Promise((resolve, reject) => {
        try {
            rl.question("Enter the id: ", (id) => {
                resolve(parseInt(id));
            });
        } catch (error) {
            reject(new Error(error));
        }
    });
}

const deleteSale = async () => {
    const id = await setIdParam();
    const apiURL = `${baseURL}/delSale?id=${id}`;

    Request.delete({
        "headers": { "content-type": "application/json" },
        "url": apiURL
    }, (error, response, body) => {
        if (error) {
            console.log(error);
        }
        else if (body) {
            // Se imprime el JSON de la venta que se acaba de dar de baja
            console.log(Util.inspect(JSON.parse(body), false, null, true));
        }
    });
}

const deleteSaleDetail = async () => {
    const id = await setIdParam();
    const apiURL = `${baseURL}/delSaleDetail?id=${id}`;

    Request.delete({
        "headers": { "content-type": "application/json" },
        "url": apiURL
    }, (error, response, body) => {
        if (error) {
            console.log(error);
        }
        else if (body) {
            // Se imprime el JSON de l detalle de venta que se acaba de dar de baja
            console.log(Util.inspect(JSON.parse(body), false, null, true));
        }
    });
}

const main = async () => {
    try {
        let choice = await menuChoice();

        switch (choice) {
            case 1:
                deleteSale();
                break;
            case 2:
                deleteSaleDetail();
                break;
            default:
                console.log(`Undefined option`);
                break;
        }
    } catch (error) {
        console.log(error);
    }
}

main();