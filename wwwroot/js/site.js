// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
//<script type="atext/javascript" src="./data/Accounts.json"></script>
function submit()
{
//this is used to get data from the add transaction section and process it to the Accounts.json file

    const ID = "FBPrinner01"; //constant for testing purposes
    let payer = document.getElementById("txtPayer").value;
    let points = document.getElementById("intPoints").value;
    var t = new Date(); //gotta really format this:
    //yyyy-MM-DDTHH:mm:ssZ
    //Need to add 1 to t.GetMonth since it is 0-11.
    //Format all data to double digits using .toLocaleString
    var t = t.getFullYear() + "-" +
        (t.getMonth() + 1).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + "-" +
        t.getDate().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + "T" +
        t.getHours().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + ":" +
        t.getMinutes().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) +":" +
        t.getSeconds().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + "Z";
    let timestamp = t;

    console.log("payer: " + payer + "\n Points:  " + points + "\n Timestamp: " + timestamp);
    //now that we have our data, we need to read in Accounts.json, get user.transactions, add our data, and update:

    //read Accounts.json:
    //var xhr = new XMLHttpRequest();
    //xhr.open('GET', './data/Accounts.json', true);
    //xhr.responseType = 'blob';
    //xhr.onload = function (e) {
    //    if (this.status == 200) {
    //        var file = new File([this.response], 'temp');
    //        var fileReader = new FileReader();
    //        fileReader.addEventListener('load', function () {
    //        });
    //        var read = fileReader.readAsText(file);
    //        console.log();
    //       );
    //    }
    //}
    //xhr.send();
}
