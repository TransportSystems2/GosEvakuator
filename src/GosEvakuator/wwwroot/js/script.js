var showView = function (viewClass) {
    var view = document.querySelector(viewClass);
    if (Boolean(view)) {
        view.classList.add("show");
    }
};

var closeView = function (viewClass) {
    var view = document.querySelector(viewClass);
    if (Boolean(view)) {
        view.classList.remove("show");
    }
};

var showModalView = function (viewClass) {
    showView(".modal_overlay");
    showView(viewClass);
};

var closeModalView = function (viewClass) {
    closeView(".modal_overlay");
    closeView(viewClass);
};

var closeAllView = function () {
    closeModalView(".modal-order");
};

var modalOverlayView = document.querySelector(".modal_overlay");
modalOverlayView.addEventListener("click", function(event){
    event.preventDefault();
    closeAllView();
});

window.addEventListener("keydown", function(event){
    if (event.keyCode === 27) {
        closeAllView();
    }
});

var orderButton = document.querySelector(".order__form-submit");
var closeBuyButton = document.querySelector(".model-order__close");

closeBuyButton.addEventListener("click", function (event) {
    event.preventDefault();
    closeModalView(".modal-order");
});

// Форма заказа эвакуатора
var orderForm = document.querySelector(".order__form");

var $orderFormPhoneNumber = $(".order__form-phone-number-value");
$orderFormPhoneNumber.inputmask("+7(999)999-99-99");
$orderFormPhoneNumber.focus(function () {
    $orderFormPhoneNumber.removeClass('invalid-value');
});

var $orderFormVehicle = $(".order__form-vehicle-value");
var $orderFormSteeringLock = $("#steering-lock-field");
var $orderFormWheelLock = $("#wheel-lock-field");
var $orderFormTotalPrice = $(".order__form-total-price-value");

var $orderFormGuaranteePrice = $(".order__guarantee-price-value");
var $orderFormGuaranteePriceFromOther = $(".order__guarantee-price-from-other-value");

var $theirPrice = $(".their-price-value");
var $ourPrice = $(".our-price-value");


var calculateTotalCost = function (price, isPriceFromOver, isLockedSteering, lockedWheelCount) {
    if (price === undefined) {
        return;
    }

    var total = isPriceFromOver ? price.LoadingVehicleFromOther : price.LoadingVehicle;

    if (isLockedSteering) {
        total += price.LockedSteeringWheel;
    }

    total += lockedWheelCount * price.LockedWheel;

    return total;
};

var recalculateTotalPrice = function () {
    var $selectedVehicle = $orderFormVehicle.find(":selected");
    var priceValue = $selectedVehicle.attr('price');
    var price = $.parseJSON(priceValue);

    var isLockedSteering = $orderFormSteeringLock.is(":checked");
    var isLockedWheel = $orderFormWheelLock.is(":checked");

    var totalPrice = calculateTotalCost(price, false, isLockedSteering, isLockedWheel);
    var totalPriceFromOther = calculateTotalCost(price, true, isLockedSteering, isLockedWheel);

    $orderFormTotalPrice.text(totalPrice);

    $orderFormGuaranteePrice.text(totalPrice);
    $orderFormGuaranteePriceFromOther.text(totalPriceFromOther);

    $theirPrice.text(totalPriceFromOther);
    $ourPrice.text(totalPrice);
};

$orderFormVehicle.change(function () {
    recalculateTotalPrice();
});

$orderFormSteeringLock.change(function () {
    recalculateTotalPrice();
});

$orderFormWheelLock.change(function () {
    recalculateTotalPrice();
});

// Окно подтверждения заказа
var $modalOrderTitle = $(".model-order__title");
var $modalOrderIndicatorMessage = $(".model-order__message");
var $modalOrderIndicatorSpinner = $(".indicator__spinner");
var $modalOrderIndicatorSuccess = $(".indicator__success");
var $modalOrderIndicatorError = $(".indicator__error");
var $modalOrderButtonClose = $(".model-order__close");
var $modalOrderContactPhone = $(".model-order__contact-phone");

orderForm.addEventListener("submit", function (event) {
    event.preventDefault();

    var isValid = Inputmask.isValid($orderFormPhoneNumber.val(), { alias: "+7(999)999-99-99" });
    if (!isValid) {
        $orderFormPhoneNumber.addClass('invalid-value');
        return;
    }

    var formData = new FormData(orderForm);

    $modalOrderTitle.text("Подача заявки");

    $modalOrderIndicatorSpinner.show();
    $modalOrderIndicatorMessage.hide();
    $modalOrderIndicatorSuccess.hide();
    $modalOrderIndicatorError.hide();
    $modalOrderButtonClose.hide();
    $modalOrderContactPhone.hide();

    showModalView(".modal-order");

    // Send the data using ajax
    $.ajax({
        type: "POST",
        url: orderForm.attributes["action"].value,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $modalOrderTitle.text("Заявка успешно принята!");

            $modalOrderIndicatorSuccess.show();
            $modalOrderIndicatorMessage.show();
            $modalOrderButtonClose.show();

            $modalOrderIndicatorSpinner.hide();
            $orderFormPhoneNumber.val('');
        },
        error: function () {
            $modalOrderIndicatorSpinner.hide();

            $modalOrderIndicatorError.show();
            $modalOrderContactPhone.show();
            $modalOrderIndicatorMessage.show();

            $modalOrderTitle.text("Нет связи с сервером");
            $modalOrderIndicatorMessage.text("В любом случае мы вам поможем, позвоните по телефону:");
        }
    });
});

// Форма расчета цены
var calcualteForm = document.querySelector(".calculate__form");
$calculateFormPhoneNumber = $(".calculate__form-phone-number-value");
$calculateFormPhoneNumber.inputmask("+7(999)999-99-99");
$calculateFormPhoneNumber.focus(function () {
    $calculateFormPhoneNumber.removeClass('invalid-value');
});

calcualteForm.addEventListener("submit", function (event) {
    event.preventDefault();

    var isValid = Inputmask.isValid($calculateFormPhoneNumber.val(), { alias: "+7(999)999-99-99" });
    if (!isValid) {
        $calculateFormPhoneNumber.addClass('invalid-value');
        return;
    }

    var formData = new FormData(calcualteForm);

    $modalOrderTitle.text("Подача заявки");

    $modalOrderIndicatorSpinner.show();
    $modalOrderIndicatorMessage.hide();
    $modalOrderIndicatorSuccess.hide();
    $modalOrderIndicatorError.hide();
    $modalOrderButtonClose.hide();
    $modalOrderContactPhone.hide();

    showModalView(".modal-order");

    // Send the data using ajax
    $.ajax({
        type: "POST",
        url: calcualteForm.attributes["action"].value,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $modalOrderTitle.text("Заявка успешно принята!");

            $modalOrderIndicatorSuccess.show();
            $modalOrderIndicatorMessage.show();
            $modalOrderButtonClose.show();

            $modalOrderIndicatorSpinner.hide();
            $orderFormPhoneNumber.val('');
        },
        error: function () {
            $modalOrderIndicatorSpinner.hide();

            $modalOrderIndicatorError.show();
            $modalOrderContactPhone.show();
            $modalOrderIndicatorMessage.show();

            $modalOrderTitle.text("Нет связи с сервером");
            $modalOrderIndicatorMessage.text("В любом случае мы вам поможем, позвоните по телефону:");
        }
    });
});