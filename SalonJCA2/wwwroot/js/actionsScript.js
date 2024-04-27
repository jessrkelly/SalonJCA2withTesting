var urllink = "";
$("#serviceid").change(function (id) {

    alert($('#serviceid :selected').text());

    window.location.href = urllink;
});


function foo(link) {
    urllink = link;


}