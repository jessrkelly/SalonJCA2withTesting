
var urllink = "";
$("#serviceid").change(function (id) {

    alert($('#serviceid :selected').text());

    window.location.href = urllink;
    //var url = "@Url.Action('Search', 'Home')?txt=kk" ;
    //alert(url);
    //    window.location.href = url;

/*    window.location.href = Url.Action("Search", "Home") + '?txt=test' ;*/

    //$.get("/Home/Search", { txt: $('#serviceid :selected').text() }, function (data) {
    //    window.location.href = data;
    //});
    
    //$.ajax({
    //    var url = '@Url.Action("Search", "Home")?txt=' + estId;
    //    window.location.href = url;
    //})
});


function foo(link) {
    /*window.location.href = link;*/
    urllink = link;


}