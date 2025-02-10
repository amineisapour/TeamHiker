jQuery(document).ready(function($) {
  "use strict";

  //Contact
  $('form.contactForm').submit(function() {

    $("#errormessage").removeClass("show");
    $("#sendmessage").removeClass("show");

    var f = $(this).find('.form-group'),
      ferror = false,
      emailExp = /^[^\s()<>@,;:\/]+@\w[\w\.-]+\.[a-z]{2,}$/i;

    f.children('input').each(function() { // run all inputs

      var i = $(this); // current input
      var rule = i.attr('data-rule');

      if (rule !== undefined) {
        var ierror = false; // error flag for current input
        var pos = rule.indexOf(':', 0);
        if (pos >= 0) {
          var exp = rule.substr(pos + 1, rule.length);
          rule = rule.substr(0, pos);
        } else {
          rule = rule.substr(pos + 1, rule.length);
        }

        switch (rule) {
          case 'required':
            if (i.val() === '') {
              ferror = ierror = true;
            }
            break;

          case 'minlen':
            if (i.val().length < parseInt(exp)) {
              ferror = ierror = true;
            }
            break;

          case 'email':
            if (!emailExp.test(i.val())) {
              ferror = ierror = true;
            }
            break;

          case 'checked':
            if (! i.is(':checked')) {
              ferror = ierror = true;
            }
            break;

          case 'regexp':
            exp = new RegExp(exp);
            if (!exp.test(i.val())) {
              ferror = ierror = true;
            }
            break;
        }
        i.next('.validation').html((ierror ? (i.attr('data-msg') !== undefined ? i.attr('data-msg') : 'wrong Input') : '')).show('blind');
      }
    });

    f.children('textarea').each(function() { // run all inputs

      var i = $(this); // current input
      var rule = i.attr('data-rule');

      if (rule !== undefined) {
        var ierror = false; // error flag for current input
        var pos = rule.indexOf(':', 0);
        if (pos >= 0) {
          var exp = rule.substr(pos + 1, rule.length);
          rule = rule.substr(0, pos);
        } else {
          rule = rule.substr(pos + 1, rule.length);
        }

        switch (rule) {
          case 'required':
            if (i.val() === '') {
              ferror = ierror = true;
            }
            break;

          case 'minlen':
            if (i.val().length < parseInt(exp)) {
              ferror = ierror = true;
            }
            break;
        }
        i.next('.validation').html((ierror ? (i.attr('data-msg') != undefined ? i.attr('data-msg') : 'wrong Input') : '')).show('blind');
      }
    });

    if (ferror) return false;

    $("#btnSubmit").css("display", "none");
    $("#imgLoading").css("display", "inline");

    
    var action = $(this).attr('action');

    if ( ! action ) {
      action = 'https://103.75.198.12:5002/gateway/contact/send';
      //action = 'https://localhost:5012/api/contact/send-data';
    }

    var formData = {
      fullname: $('#fullname').val(),
      email: $('#email').val(),
      subject: $('#subject').val(),
      message: $('#message').val()
  };

  //var str = $(this).serialize();
  var str = JSON.stringify(formData);

    $.ajax({
      type: "POST",
      url: action,
      contentType: "application/json",
      data: str,
      cache: false,
      success: function(response) {
        //alert(msg);
        //console.log(response)

        $("#btnSubmit").css("display", "inline");
        $("#imgLoading").css("display", "none");

        if (response.isSuccess) {
          $("#sendmessage").addClass("show");
          $("#errormessage").removeClass("show");
          $('.contactForm').find("input, textarea").val("");
        } else {
          var msgTxt = response.errors.join(", ");
          $("#sendmessage").removeClass("show");
          $("#errormessage").addClass("show");
          $('#errormessage').html(msgTxt);
        }
      },
      error: function (textStatus, errorThrown) {
          console.log(textStatus);
          $("#sendmessage").removeClass("show");
          $("#errormessage").addClass("show");
          $('#errormessage').html("There is an Error, Please try again later.");

          $("#btnSubmit").css("display", "inline");
          $("#imgLoading").css("display", "none");
      }
    });

    // Email.send({
    //       Host: "mail.teamhiker.com:587",
    //       Username: "info@teamhiker.com",
    //       Password: "Mahdi@933",
    //       To: $("#email").val(),
    //       From:"info@teamhiker.com",
    //       Subject: $("#subject").val(),
    //       Body:   $("#name").val()+ ':\r\n' + $("#message").val(),
    //       // Attachments: [
    //       //     {
    //       //         name: "File_Name_with_Extension",
    //       //         path: "Full Path of the file"
    //       //     }]
    //   })
    //   .then(function (message) {
    //     $("#sendmessage").addClass("show");
    //     $("#errormessage").removeClass("show");
    //     $('.contactForm').find("input, textarea").val("");
    //   });


    return false;
  });

});
