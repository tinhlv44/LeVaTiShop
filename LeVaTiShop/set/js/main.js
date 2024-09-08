// check
function check_tel(tel) {
    var vnf_regex = /((09|03|07|08|05)+([0-9]{8})\b)/g;
    return vnf_regex.test(tel.value);
}

function presen_modal(content) {
    var presen = document.getElementById("click_modal");
    presen.style.display = "flex";
    presen.innerHTML = "\
        <div class='modal_box'>\
            <span class='modalbox_close'>&times;</span>\
            <h3 class='modalbox_heading'>Thông báo</h3>\
            <p class='modalbox_ct'></p>\
        </div>"
    document.getElementsByClassName("modalbox_ct")[0].innerHTML = content;
    document.getElementsByClassName("modalbox_close")[0].onclick = function () {
        presen.style.display = "none";
    }
    window.onclick = function (event) {
        if (event.target == presen) {
            presen.style.display = "none";
        }
    }
}
function not_login() {
    presen_modal("Tín năng kết nối bằng mạng xã hội chưa khả dụng.<br>Vui lòng thử lại sau!");
}
function hiddenpass() {
    var x = document.getElementById("relo_input_pass");
    var y = document.getElementById("relo_input_repass");
    var z = document.getElementById("relo_input_repass2");
    if (x.type === "password") {
        x.type = "text";
        y.type = "text";
        z.type = "text";
    }
    else {
        x.type = "password";
        y.type = "password";
        z.type = "password";
    }
}
// slide show 
/*function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let slides = document.getElementsByClassName("myslide");
    let dots = document.getElementsByClassName("slide_idd");

    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }

    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    for (let i = 0; i < dots.length; i++) {
        dots[i].classList.remove("active");
    }

    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].classList.add("active");
}*/
// see more
function seemore() {
    let see = document.getElementById("seemore");
    let hide = document.getElementsByClassName("hide");
    see.style.display = "none";
    for (let i = 0; i < hide.length; i++) {
        hide[i].style.display = "block";
    }
}

function hide() {
    let see = document.getElementById("seemore");
    let hide = document.getElementsByClassName("hide");
    see.style.display = "block";
    for (let i = 0; i < hide.length; i++) {
        hide[i].style.display = "none";
    }
}
// click seletion
function selection(ct, n, path, amount) {
    /*element.style.border = "2px solid red";*/
    var x = document.getElementsByClassName("proi_box" + ct);
    var a = document.getElementById('amount');
    a.innerHTML = "Số lượng còn: " + amount;
    /*if (amount == "0") {
        var fc = document.getElementById('fastCheck');
        var pc = document.getElementById('prot_cart');
        fc.href = 
    }*/
    if ("proi_box" + ct == "proi_boxcolor") {
        change(path);
    }
    for (let i = 0; i < x.length; i++) {
        x[i].classList.remove('selectioned');
    }
    x[n].classList.add('selectioned');
}
// change img
function change(path) {
    var y = document.getElementsByClassName("proinfor_cimg");
    y[0].src = "/set/img/product/" + path;
    var startIndex = path.lastIndexOf("/") + 1; // Tìm vị trí cuối cùng của dấu "/"
    var fileName = path.substr(startIndex); // Tách tên file từ vị trí đó đến hết chuỗi
    var imgCart = document.getElementById("prot_cart");
    var fastCheck = document.getElementById("fastCheck");
    imgCart.setAttribute("data-product-image", fileName);
    fastCheck.setAttribute("data-product-image", fileName);
}
// noti
function not_buy() {
    var x = document.getElementsByClassName("buy_in");
    var y = document.getElementsByClassName("notibuy");
    for (var i = 0; i < x.length; i++) {
        if (i != 2 && i != 4 && x[i].value == "") {
            y[0].style.display = "block";
            x[i].focus();
            return false;
        }
    }
    if (!check_tel(x[1])) {
        y[0].innerHTML = "Số điện thoại không hợp lệ!"
        y[0].style.display = "block";
        x[1].focus();
        return false;
    }
    y[0].style.display = "none";
    presen_modal("Bạn đã mua hàng thành công!<br>Hàng lúc nào về thì chúng tôi không biết!");
    return true;
}
function out_of_stock() {
    presen_modal("Chưa có hàng bạn ơi!");
}
function not_promotion() {
    presen_modal("Hiện chưa có thông tin khuyến mãi.");
}
function go_home() {
    presen_modal("Vui lòng quay về trang chủ!");
}
function end_stock() {
    presen_modal("Đã hết hàng.");
}

// hide product
function hide_mb() {
    var mbstyle = document.getElementsByClassName("product_type");
    for (let i = 0; i < mbstyle.length; i++) {
        mbstyle[i].style.display = "none";
    }
}
function presen_type(type) {
    hide_mb();
    document.getElementById("type_" + type).style.display = "block";
}

// buy
function buying_stock() {
    var presen = document.getElementById("click_modal");
    presen.style.display = "flex";
    presen.innerHTML = "\
            <div class='buying'>\
                <ul class='buy_list'>\
                    <li class='buy_item'>\
                        <h3 class='buy_heading'>Thông tin khách hàng</h3>\
                    </li>\
                    <li class='buy_item notibuy'>\
                        <p class='buy_noti'>Bạn vui lòng điền đầy đử thông tin và chính xác để nhận hàng.</p>\
                    </li>\
                    <li class='buy_item'>\
                        <p class='forcus'>*</p><input type='text' class='buy_in' placeholder='Họ và tên'>\
                    </li>\
                    <li class='buy_item'>\
                        <p class='forcus'>*</p><input type='text' class='buy_in' placeholder='Số điện thoại'>\
                    </li>\
                    <li class='buy_item'>\
                        <input type='text' class='buy_in' placeholder='Email'>\
                    </li>\
                    <li class='buy_item'>\
                        <p class='forcus'>*</p><input type='text' class='buy_in' placeholder='Địa chỉ nhận hàng (Vui lòng ghi rõ ràng)'>\
                    </li>\
                    <li class='buy_item'>\
                        <input type='text' class='buy_in' placeholder='Yêu cầu khác'>\
                    </li>\
                    <li class='buy_item'><button class='btn btn_green buy_btn' onclick='not_buy()'>Mua hàng</button></li>\
                </ul>\
                <span class='modalbox_close close_buy'>&times;</span>\
            </div>";
    document.getElementsByClassName("modalbox_close")[0].onclick = function () {
        presen.style.display = "none";
    }
    window.onclick = function (event) {
        if (event.target == presen) {
            presen.style.display = "none";
        }
    }
}

function hide_Model(modal) {
    var presen = document.getElementById(modal);
    document.getElementsByClassName("modalbox_close")[0].onclick = function () {
        presen.style.display = "none";
    }
    window.onclick = function (event) {
        if (event.target == presen) {
            presen.style.display = "none";
        }
    }
}

function clear_stock() {
    var check = document.getElementsByClassName("not_check");
    var pcart = document.getElementsByClassName("pcart_list")[0];
    let i;
    for (i = 0; i < check.length; i++) {
        while (check[i].checked === true) {
            pcart.removeChild(document.getElementsByClassName("pcart_item")[i]);
            if (check[0] == undefined) {
                document.getElementsByClassName("pcart_hollow")[0].style.display = "flex";
                document.getElementsByClassName("pcart_prot")[0].style.display = "none";
                document.getElementById("cart_icon").style.display = "none";
                return true;
            }
        }
    }
}

// add_cart
cnt = 0;
function add_cart(n) {
    document.getElementsByClassName("pcart_hollow")[0].style.display = "none";
    document.getElementsByClassName("pcart_prot")[0].style.display = "flex";
    document.getElementById("cart_icon").style.display = "block";
    var cart = document.getElementById("cart");
    cart.innerHTML = cart.innerHTML + "<li class='pcart_item'>\
                                            <div class='pcart_checkbox'>\
                                                <label for='c"+ cnt + "' class='checkbox_box'>\
                                                    <input type='checkbox' class='not_check' id='c"+ cnt + "'>\
                                                    <i class='checkboxic fa-solid fa-check' ></i>\
                                                </label>\
                                            </div>\
                                            <img src='' alt='' class='pcart_img' id='cimg'>\
                                            <div class='pcart_infor'>\
                                                <h3 class='pcart_name' id='cname'></h3>\
                                                <div class='pcart_coin'>\
                                                    <div class='ccoinpre' id='cpre' style='font-size: 16px;'></div>\
                                                    <div class='ccoinpast' id='cpast' style='font-size: 16px;'></div>\
                                                    <!-- <div class='pcart_ram' style='font-size: 16px;'>6G/128G</div>--> \
                                                </div>\
                                            </div>\
                                        </li>   ";
    document.getElementById("cimg").src = document.getElementsByClassName("prot_img")[n].src;
    document.getElementById("cname").innerHTML = document.getElementsByClassName("prot_heading")[n].innerHTML;
    document.getElementById("cpre").innerHTML = document.getElementsByClassName("coinpre")[n].innerHTML;
    document.getElementById("cpast").innerHTML = document.getElementsByClassName("coinpast")[n].innerHTML;
    document.getElementById("cimg").id = '';
    document.getElementById("cname").id = '';
    document.getElementById("cpre").id = '';
    document.getElementById("cpast").id = '';
    cnt += 1;
}
function add_cart2() {
    document.getElementsByClassName("pcart_hollow")[0].style.display = "none";
    document.getElementsByClassName("pcart_prot")[0].style.display = "flex";
    document.getElementById("cart_icon").style.display = "block";
    var cart = document.getElementById("cart");
    cart.innerHTML = cart.innerHTML + "<li class='pcart_item'>\
                                            <div class='pcart_checkbox'>\
                                                <label for='c"+ cnt + "' class='checkbox_box'>\
                                                    <input type='checkbox' class='not_check' id='c"+ cnt + "'>\
                                                    <i class='checkboxic fa-solid fa-check' ></i>\
                                                </label>\
                                            </div>\
                                            <img src='' alt='' class='pcart_img' id='cimg'>\
                                            <div class='pcart_infor'>\
                                                <h3 class='pcart_name' id='cname'></h3>\
                                                <div class='pcart_coin'>\
                                                    <div class='ccoinpre' id='cpre' style='font-size: 16px;'></div>\
                                                    <div class='ccoinpast' id='cpast' style='font-size: 16px;'></div>\
                                                    <!-- <div class='pcart_ram' style='font-size: 16px;'>6G/128G</div>--> \
                                                </div>\
                                            </div>\
                                        </li>   ";
    document.getElementById("cimg").src = document.getElementsByClassName("proinfor_cimg")[0].src;
    document.getElementById("cname").innerHTML = document.getElementsByClassName("proinfor_heading")[0].innerHTML;
    document.getElementById("cpre").innerHTML = document.getElementsByClassName("coinpre")[0].innerHTML;
    document.getElementById("cpast").innerHTML = document.getElementsByClassName("coinpast")[0].innerHTML;
    document.getElementById("cimg").id = '';
    document.getElementById("cname").id = '';
    document.getElementById("cpre").id = '';
    document.getElementById("cpast").id = '';
    cnt += 1;
}


//listdown
function showAndActive(id) {
    var check = document.getElementById(id);
    if (check.style.display == "block") {
        check.style.display = "none";
        document.getElementsByClassName("dropdown")[0].remove.add("chooseLogin");
    }
    else {
        check.style.display = "block";
        document.getElementsByClassName("dropdown")[0].classList.add("chooseLogin");
    }
}
/*function showAndHide(id) {
    show(id);
    document.addEventListener("click", hideDiv);
}


function hideDiv(event) {
    var div = document.getElementById("myDropdown");
    var target = event.target;
    if (target !== div && !div.contains(target)) {
        div.style.display = "none";
        document.removeEventListener("click", hideDiv);
    }
}*/

function show(id) {
    var check = document.getElementById(id);
    if (check.style.display == "block") {
        check.style.display = "none";
    }
    else {
        check.style.display = "block";
    }
}




//time
function displayCurrentTime() {
    var now = new Date();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    var dayOfWeek = now.toLocaleDateString('vi-VN', { weekday: 'long' });
    var date = now.toLocaleDateString('vi-VN', { year: 'numeric', month: 'long', day: 'numeric' });

    // Định dạng giờ, phút, giây thành chuỗi có độ dài 2 và thêm '0' nếu cần
    hours = addLeadingZero(hours);
    minutes = addLeadingZero(minutes);
    seconds = addLeadingZero(seconds);

    // Hiển thị giờ, phút, giây, thứ và ngày hiện tại trong một phần tử HTML với id="current-time"
    var currentTime = hours + ":" + minutes + ":" + seconds + " - " + dayOfWeek + ", " + date;
    document.getElementById("date").innerHTML = currentTime;
}

function addLeadingZero(number) {
    return (number < 10 ? "0" : "") + number;
}


//intput số định nghĩa hàng nghìn


//hàm thêm ảnh sản phẩm
function displayImages(input) {
    var imageContainer = document.getElementById("imageContainer");
    imageContainer.innerHTML = "";

    if (input.files && input.files.length > 0) {
        for (var i = 0; i < input.files.length; i++) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var imgElement = document.createElement("img");
                imgElement.src = e.target.result;
                imgElement.alt = "Uploaded Image";
                imgElement.style.maxWidth = "200px";
                imgElement.style.maxHeight = "200px";
                imageContainer.appendChild(imgElement);

                var colorInput = document.createElement("input");
                colorInput.type = "color";
                colorInput.name = "colors";
                imageContainer.appendChild(colorInput);

                var amountInput = document.createElement("input");
                amountInput.type = "text";
                amountInput.name = "amount";
                amountInput.maxlength = "5";
                amountInput.value = "0";
                amountInput.classList.add("form-control");
                amountInput.classList.add("numeric-input");
                imageContainer.appendChild(amountInput);
            }
            reader.readAsDataURL(input.files[i]);
        }
    }
}


//Bỏ kí tu dac biet
function restrictSpecialCharacters(input) {
    input.value = input.value.replace(/[^a-zA-Z0-9]/g, '');
}
function showId(id) {
    document.getElementById(id).style.display = 'block';
}