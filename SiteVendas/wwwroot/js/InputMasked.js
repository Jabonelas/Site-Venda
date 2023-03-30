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
        numericOnly: true,
    });
}

if (document.querySelector("#cleave-celular")) {
    var cleaveTelefoneRecado = new Cleave("#cleave-celular", {
        delimiters: ["(", ") ", "-", "-"],
        blocks: [0, 2, 5, 4],
        numericOnly: true,
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
                    numericOnly: true,
                });
            }
        });
}

if (document.querySelector("#cleave-telefone")) {
    var cleaveTelefone = new Cleave("#cleave-telefone", {
        delimiters: ["(", ") ", "-", "-"],
        blocks: [0, 2, 5, 4],
        numericOnly: true,
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
                    numericOnly: true,
                });
            }
        });
}

//if (document.querySelector("#cleave-dinheiro")) {
//    var cleaveDate = new Cleave("#cleave-dinheiro", {
//        delimiters: ["R$ "],
//        blocks: [0, 9],
//        //delimiters: ["R$ ", ","],
//        //blocks: [0, 3, 2],
//        //numericOnly: true,
//    });
//}

//if (document.querySelector("#cleave-dinheiro")) {
//    var cleavePrefix = new Cleave("#cleave-dinheiro", {
//        delimiters: ["R$ ", ","],
//        blocks: [0, 3, 2],
//        numericOnly: true,
//        onValueChanged: function (e) {
//            var value = e.target.value;
//            value = value.replace(/[^0-9]/g, ""); // remove tudo que não for número
//            value = value.replace(/^0+/g, ""); // remove zeros à esquerda
//            value = value.padStart(3, "0"); // adiciona zeros à esquerda, se necessário
//            value = value.replace(/(\d{1,3})$/, ",$1"); // adiciona vírgula antes dos últimos 2 dígitos
//            value = value.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1."); // adiciona ponto a cada 3 dígitos
//            e.target.value = "R$ " + value; // adiciona o prefixo "R$ " e atualiza o valor do campo
//        },
//    });
//}

if (document.querySelector("#cleave-dinheiro")) {
    var cleavePrefix = new Cleave("#cleave-dinheiro", {
        prefix: "R$ ",
        numeral: true,
        numeralDecimalScale: 2,
        delimiter: ".",
        numeralDecimalMark: ",",
        onValueChanged: function (e) {
            var value = e.target.value.replace(/^R\$\s/g, ""); // remove o prefixo "R$ "
            value = value.replace(/\./g, ""); // remove os pontos
            value = value.replace(/[^0-9,]/g, ""); // remove tudo que não for número ou vírgula
            var parts = value.split(",");
            var integerPart = parts[0] || "0";
            var decimalPart = parts[1] || "";
            integerPart = integerPart.replace(/^0+/g, ""); // remove zeros à esquerda
            decimalPart = decimalPart.padEnd(2, "0").substring(0, 2); // completa com zeros à direita e limita a duas casas decimais
            value = integerPart + (decimalPart ? "," + decimalPart : ""); // adiciona a vírgula, se houver casas decimais
            e.target.value = "R$ " + value; // adiciona o prefixo "R$ " e atualiza o valor do campo
        },
    });
}


if (document.querySelector("#cleave-cnpj")) {
    var cleaveDate = new Cleave('#cleave-cnpj', {
        delimiters: ['.', '.', '/', '-'],
        blocks: [2, 3, 3, 4, 2]
    });
}

if (document.querySelector("#cleave-horarioInicio")) {
    var cleaveHorario = new Cleave('#cleave-horarioInicio', {
        time: true,
        timePattern: ['h', 'm']
    });
}

if (document.querySelector("#cleave-horarioFechamento")) {
    var cleaveHorario = new Cleave('#cleave-horarioFechamento', {
        time: true,
        timePattern: ['h', 'm']
    });
}
