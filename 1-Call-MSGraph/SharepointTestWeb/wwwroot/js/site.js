// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var app = new Vue({
    el: '#app',
    data: {
        message: 'Hello Vue!',
        fieldData: {
            fields: {
                Title: "Alextest",
                Organization: "CPSC-EXIT",
                Phone: "301-504-7804",
                Source: "sharepointOnline-GraphAPI",
                Email: "asalomon@cpsc.gov",
                Message: "<div>Markers.&#160; Have we satisfied all of the CPSIA requirements.&#160; </div>\n<div>&#160;</div>\n<div>Message = Hello, we are in the process of having dry erase markers manufactured in the US and want to be sure that our products meet all CPSIA requirements. We were informed that they are ASMT D4236 approved, but not sure if there are other testing that needs to be done to make sure they are completely safe for children. Thank you for your time, we look forward to hearing from you. Best, Joclynn and team</div>\n<div>&#160;</div>\n<div>Messsage Received Oct. 21st, 2&#58;44 pm, Retrieved Nov. 4th</div>",
                Comments: "<div>Also rec'd email from 10/21/10, retreived on 11/4/10, called back on 11/29/10 and it seems as thought they figured out the answer in the interim.&#160; Left the message that no further testng needed. </div>"
            }
        },

        responseM: {}

    },
    methods: {
        async send() {
            console.log(this.responseM)
            this.responseM = await this.submit()
            console.log(this.responseM)

        },
       async  submit() {
            const url = '/home/submit'
            const data = this.fieldData
            let vm = this
            // Default options are marked with *
            const response = await fetch(url, {
                method: 'POST', // *GET, POST, PUT, DELETE, etc.
               // mode: 'cors', // no-cors, *cors, same-origin
                cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                //credentials: 'same-origin', // include, *same-origin, omit
                headers: {
                    'Content-Type': 'application/json'
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
               // redirect: 'follow', // manual, *follow, error
               // referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                body: JSON.stringify(data) // body data type must match "Content-Type" header
            }).then(function (response) {
                vm.responseM = response.json()
                return response.json()
 
            }).catch(function (error) {
               vm.responseM = error
            }).finally(function () {
                vm.responseM = {"message":"nothing happened"}
            });







        }
    }
   
})
