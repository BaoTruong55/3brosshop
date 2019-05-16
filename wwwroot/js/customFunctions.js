/*-------------------------------------
Scroll disablen
-------------------------------------*/

var keys = { 37: 1, 38: 1, 39: 1, 40: 1 };

function preventDefault(e) {
    e = e || window.event;
    if (e.preventDefault)
        e.preventDefault();
    e.returnValue = false;
}

function preventDefaultForScrollKeys(e) {
    if (keys[e.keyCode]) {
        preventDefault(e);
        return false;
    }
}

function disableScroll() {
    if (window.addEventListener) // older FF
        window.addEventListener('DOMMouseScroll', preventDefault, false);
    window.onwheel = preventDefault; // modern standard
    window.onmousewheel = document.onmousewheel = preventDefault; // older browsers, IE
    window.ontouchmove = preventDefault; // mobile
    document.onkeydown = preventDefaultForScrollKeys;
}

function enableScroll() {
    if (window.removeEventListener)
        window.removeEventListener('DOMMouseScroll', preventDefault, false);
    window.onmousewheel = document.onmousewheel = null;
    window.onwheel = null;
    window.ontouchmove = null;
    document.onkeydown = null;
}

/*-------------------------------------
Winkelmand index acties
-------------------------------------*/

function winkelmandAantalVerhogen(id, price) {
    var plusId = id;
    var plusPrijs = price;

    $.ajax({
        type: "GET",
        url: '/ShoppingCart/Plus',
        data: { Id: plusId, Price: plusPrijs }
    }).done(function (result) {
        $("#winkelwagen-list-partial").html(result);
        updateShoppingCartCount();
    });


};

function winkelmandAantalVerlagen(id, price) {
    var plusId = id;
    var plusPrice = price;

    $.ajax({
        type: "GET",
        url: '/ShoppingCart/Min',
        data: { Id: plusId, Price: plusPrice }
    }).done(function (result) {
        $("#winkelwagen-list-partial").html(result);
        updateShoppingCartCount();
    });
};

function winkelmandItemVerwijderen(id, price) {
    if (confirm("Bạn có chắc chắn muốn xóa (các) mặt hàng này khỏi giỏ hàng của bạn?") == true) {
        var plusId = id;
        var plusPrice = price;

        $.ajax({
            type: "GET",
            url: '/ShoppingCart/Remove',
            data: { Id: plusId, Price: plusPrice }
        }).done(function (result) {
            $("#winkelwagen-list-partial").html(result);
            updateShoppingCartCount();
        });
    }
};

/*-------------------------------------
Winkelmand count span acties acties
-------------------------------------*/

function updateShoppingCartCount() {
    $.ajax({
        type: "GET",
        url: '/Home/UpdateShoppingCartCount'
    }).done(function (result) {
        $(".layout-winkelwagen-count").html(result);
    });
}

/*-------------------------------------
acties voor het toevoegen aan winkelmand
-------------------------------------*/

function DynamicAddCartRegion(id) {
    $('#' + id + "default").slideToggle('slow', function () {
        $('#' + id + "default").toggleClass('selecionado', $(this).is(':visible'));
    });
    $('#' + id + "card").slideToggle('slow', function () {
        $('#' + id + "card").toggleClass('selecionado', $(this).is(':visible'));
    });
}

function AddToShoppingCartBasket(id, image, name) {
    try {
        console.log("123");
        var id = id;
        var image = image;
        var name = name;
        var price = parseFloat(document.getElementById(id + "PriceField").value.replace(',', '.')).toFixed(2);
        var number = document.getElementById(id + "QuantityField").value;

        showAddShoppingCartPopup(image, name, price, number);

        $.ajax({
            type: "GET",
            url: '/ShoppingCart/Add',
            data: { id: id, price: price, number: number }
        }).done(function () {
            updateShoppingCartCount();
            document.getElementById((id + "TerugKnop")).click();
        });
        return false;
    } catch (e) {
        return false;
    }
}

function hideAddedToShoppingBasketPopup() {
    document.getElementById('addedToShoppingCartPopup').style.display = 'none';
    document.getElementById("pageWrapperForBlur").classList.remove('blurOverlay');
    enableScroll();
}

function showAddShoppingCartPopup(image, name, price, number) {
    document.getElementById("shopBasketPopupItemsName").innerHTML = '<i class="far fa-check-circle font-26"></i> ' + name + ' werd toegevoegd aan uw winkelwagen!';
    document.getElementById("shopBasketPopupItemsImage").src = "/" + image;
    document.getElementById("shopBasketPopupItemsValue").innerHTML = "- Số tiền: " + price + "vnđ";
    document.getElementById("shopBasketPopupItemsNumber").innerHTML = "- Số lượng: " + number;

    document.getElementById("pageWrapperForBlur").classList.add("blurOverlay");

    document.getElementById('addedToShoppingCartPopup').style.display = 'block';

    document.getElementById("closeShopBasketPopup").onclick = function () {
        hideAddedToShoppingBasketPopup()
    }

    window.onclick = function (event) {
        if (event.target == document.getElementById('addedToShoppingCartPopup')) {
            hideAddedToShoppingBasketPopup()
        }
    }

    disableScroll();
}

$(".winkelmand-register-enter").keyup(function (event) {
    if (event.keyCode === 13) {
        var idVanWinkelMandTrigger = event.target.id;
        var idVanKnopWinkelMandVoorTrigger = "." + idVanWinkelMandTrigger.toString().replace("PriceField", "").replace("QuantityField", "") + "BevestigKnop"
        document.getElementById(idVanKnopWinkelMandVoorTrigger).click();
    }
});

/*-------------------------------------
Animaties callen maar niet op mobile
-------------------------------------*/

$(function () {
    wow = new WOW(
        {
            mobile: false
        }
    )
    wow.init();
});