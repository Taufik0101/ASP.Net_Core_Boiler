require("dotenv").config()
const { createServer } = require("http");

const httpServer = createServer();

const express = require('express')
const bodyParser = require('body-parser')
const app = express()
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json())

const lowerCharSet   = "abcdedfghijklmnopqrst"
const upperCharSet   = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
const specialCharSet = "!@#$%&*"
const numberSet      = "0123456789"

const argon2 = require('argon2');

app.post('/generateRandomPassword', async function (req, res) {
    let lowerLength = req.body.LowerLength;
    let upperLength = req.body.UpperLength;
    let numLength = req.body.NumLength;
    let specialLength = req.body.SpecialLength;

    let passwordGenerate = "";

    for (let i = 0; i < lowerLength; i++) {
        passwordGenerate += lowerCharSet[~~(Math.random() * lowerLength)]
    }

    for (let i = 0; i < upperLength; i++) {
        passwordGenerate += upperCharSet[~~(Math.random() * upperLength)]
    }

    for (let i = 0; i < numLength; i++) {
        passwordGenerate += numberSet[~~(Math.random() * numLength)]
    }

    for (let i = 0; i < specialLength; i++) {
        passwordGenerate += specialCharSet[~~(Math.random() * specialLength)]
    }

    let hashedPassword = await argon2.hash(passwordGenerate);
    
    res.status(200).json({
        message: 'Password Successfully Generated',
        data: {
            password: passwordGenerate,
            hashed: hashedPassword,
        },
    })
})

app.post('/verifyPassword', async function (req, res) {
    let plainPassword = req.body.Password;
    let hashedPassword = req.body.Hashed;

    if (await argon2.verify(hashedPassword, plainPassword)) {
        // password match
        res.status(200).json({
            message: 'Password Match',
            verify: true
        })
    } else {
        // password did not match
        res.status(400).json({
            message: "Password Doesn't Match",
            verify: false
        })
    }
})

app.post('/hashPassword', async function (req, res) {
    let plainText = req.body.Password;
    let hashedPassword = await argon2.hash(plainText);
    res.status(200).json({
        message: 'Password Successfully Generated',
        data: hashedPassword
    })
})

app.listen(process.env.PORT_API)
console.log("Api connect to http://localhost:%s", process.env.PORT_API)