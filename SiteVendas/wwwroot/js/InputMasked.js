if (document.querySelector("#cleave-cpf")) {
    var cleave = new Cleave("#cleave-cpf", {
        delimiters: [".", ".", "-"],
        blocks: [3, 3, 3, 2],
        numericOnly: true,
    });
}

if (document.querySelector("#cleave-rg")) {
    var cleave = new Cleave("#cleave-rg", {
        delimiters: [".", ".", "-"],
        blocks: [2, 3, 3, 1],
        numericOnly: true,
    });
}

if (document.querySelector("#cleave-date")) {
    var cleaveDate = new Cleave("#cleave-date", {
        date: true,
        delimiter: "/",
        datePattern: ["d", "m", "Y"],
    });
}

if (document.querySelector("#cleave-nome")) {
    var cleave = new Cleave("#cleave-nome", {
        blocks: [100],
        uppercase: true,
        delimiter: '',
        numericOnly: false,
        prefix: '',
        noImmediatePrefix: true,
        rawValueTrimPrefix: true
    });
}

if (document.querySelector("#cleave-cep1")) {
    var cleaveDate = new Cleave("#cleave-cep1", {
        delimiters: ["-"],
        blocks: [5, 3],
    });
}

if (document.querySelector("#cleave-celular")) {
    var cleaveTelefoneRecado = new Cleave("#cleave-celular", {
        delimiters: ["(", ") ", "-", "-"],
        blocks: [0, 2, 5, 4],
    });

    document
        .querySelector("#cleave-celular")
        .addEventListener("input", function (data) {
            if (data.inputType == "deleteContentBackward") {
                cleaveTelefoneRecado.destroy();
            } else {
                cleaveTelefoneRecado.destroy();
                cleaveTelefoneRecado = new Cleave("#cleave-celular", {
                    delimiters: ["(", ") ", "-", "-"],
                    blocks: [0, 2, 5, 4],
                });
            }
        });
}

if (document.querySelector("#cleave-telefone")) {
    var cleaveTelefone = new Cleave("#cleave-telefone", {
        delimiters: ["(", ") ", "-", "-"],
        blocks: [0, 2, 5, 4],
    });

    document
        .querySelector("#cleave-telefone")
        .addEventListener("input", function (data) {
            if (data.inputType == "deleteContentBackward") {
                cleaveTelefone.destroy();
            } else {
                cleaveTelefone.destroy();
                cleaveTelefone = new Cleave("#cleave-telefone", {
                    delimiters: ["(", ") ", "-", "-"],
                    blocks: [0, 2, 5, 4],
                });
            }
        });
}