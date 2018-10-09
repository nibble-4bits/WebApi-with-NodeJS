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
            rl.question("Choose an option\n1. Get sales by product ID\n2. Get sales by date\nOPTION: ", (choice) => {
                resolve(parseInt(choice));
            });
        } catch (error) {
            reject(new Error(error));
        }
    });
}

const setProductIdParam = () => {
    return new Promise((resolve, reject) => {
        try {
            rl.question("Enter the product id: ", (id) => {
                resolve(parseInt(id));
            });
        } catch (error) {
            reject(new Error(error));
        }
    });
}

const setDateParam = () => {
    return new Promise((resolve, reject) => {
        try {
            rl.question("Enter the date (YYYY-MM-DD): ", (date) => {
                resolve(date);
            });
        } catch (error) {
            reject(new Error(error));
        }
    });
}

const getSalesByProductId = async () => {
    const productId = await setProductIdParam();
    const apiURL = `${baseURL}?productId=${productId}`;

    Request.get(apiURL, (error, response, body) => {
        if (error) {
            console.log(error);
        }
        else {
            console.log(Util.inspect(JSON.parse(body), false, null, true));
        }
    });
}

const getSalesByDate = async () => {
    const date = await setDateParam();
    const apiURL = `${baseURL}?date=${date}`;

    Request.get(apiURL, (error, response, body) => {
        if (error) {
            console.log(error);
        }
        else {
            console.log(Util.inspect(JSON.parse(body), false, null, true));
        }
    });
}

const main = async () => {
    try {
        let choice = await menuChoice();

        switch (choice) {
            case 1:
                getSalesByProductId();
                break;
            case 2:
                getSalesByDate();
                break;
            default:
                process.kill(0);
                // console.log(`Undefined option`);
                // break;
        }
    } catch (error) {
        console.log(error);
    }
}

main();