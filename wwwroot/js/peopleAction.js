
//Option to remove fone
function RemovePhone(id) {
    var buttonRemove = document.getElementById(id);
    var divMenor = buttonRemove.parentNode;
    var divPhone = document.getElementById("_phone");
    divPhone.removeChild(divMenor);
}

//Add on create space to add new phone
$("#NewNumber").on("click", function () {
    $.ajax({
        url: '~/CreatePartial',
        success: function (view) {
            var buttId = Math.random() * 10;
            var divFone = document.createElement('div');
            divFone.className = 'form-group col-8';

            var buttonRemove = document.createElement('button');
            var text = document.createTextNode(' X ');

            buttonRemove.type = 'button';
            buttonRemove.id = 'buttId' + buttId;
            buttonRemove.setAttribute('onclick', 'RemovePhone(this.id)');
            buttonRemove.className = 'btn btn-danger';

            buttonRemove.appendChild(text);

            divFone.appendChild(buttonRemove);
            divFone.insertAdjacentHTML('afterbegin', view);

            var divPhone = document.getElementById("_phone");
            divPhone.appendChild(divFone);
        }
    });
});

function completeState(state) {
    switch (state) {
        case "AC":
            state = "Acre";
            break;
        case "AL":
            state = "Alagoas";
            break;
        case "AP":
            state = "Amapá";
            break;
        case "AM":
            state = "Amazonas";
            break;
        case "BA":
            state = "Bahia";
            break;
        case "CE":
            state = "Ceará";
            break;
        case "DF":
            state = "Distrito Federal";
            break;
        case "ES":
            state = "Espírito Santo";
            break;
        case "GO":
            state = "Goiás";
            break;
        case "MA":
            state = "Maranhão";
            break;
        case "MT":
            state = "Mato Grosso";
            break;
        case "MS":
            state = "Mato Grosso do Sul";
            break;
        case "MG":
            state = "Minas Gerais";
            break;
        case "PA":
            state = "Pará";
            break;
        case "PB":
            state = "Paraíba";
            break;
        case "PR":
            state = "Paraná";
            break;
        case "PE":
            state = "Pernambuco";
            break;
        case "PI":
            state = "Piauí";
            break;
        case "RJ":
            state = "Rio de Janeiro";
            break;
        case "RN":
            state = "Rio Grande do Norte";
            break;
        case "RS":
            state = "Rio Grande do Sul";
            break;
        case "RO":
            state = "Rondônia";
            break;
        case "RR":
            state = "Roraima";
            break;
        case "SC":
            state = "Santa Catarina";
            break;
        case "SP":
            state = "São Paulo";
            break;
        case "SE":
            state = "Sergipe";
            break;
        case "TO":
            state = "Tocantins";
            break;
    }
    return state;
}

const librarian = ("#IsEmployer");

$(document).ready(function () {

    var password = $("#Password").val();

    if (Boolean(password)) {
        $("#divPassword").show();
        $(librarian).attr('checked', true);
    } else {
        $("#divPassword").hide();
        $(librarian).removeAttr('checked', false);
    }
});

$(librarian).change(function () {
    if (!this.checked) {
        $("#divPassword").hide();
    } else {
        $("#divPassword").show();
    }
});
