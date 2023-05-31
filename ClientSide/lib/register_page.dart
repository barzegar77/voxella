import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'services/ApiService.dart';
import 'dart:convert';
import 'package:flutter/gestures.dart';
import 'terms_and_conditions_page.dart';
import 'email_confirmation_page.dart';

class RegisterPage extends StatefulWidget {
  @override
  _RegisterPageState createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  TextEditingController _usernameController = TextEditingController();
  TextEditingController _emailController = TextEditingController();
  TextEditingController _passwordController = TextEditingController();

  ApiService _apiService = ApiService();

  String? usernameErrorText;
  String? emailErrorText;
  String? passwordErrorText;

 Future<void> registerUser() async {
  var endpoint = 'Authentication/register';
  var data = {
    'username': _usernameController.text,
    'email': _emailController.text,
    'password': _passwordController.text,
  };

  try {
    var response = await _apiService.post(endpoint, data);

    if (response.statusCode == 200) {
      var responseBody = jsonDecode(response.body);
      if (responseBody["status"]) {
        final String email = _emailController.text;
        Get.snackbar(
          'Success',
          responseBody["message"],
          backgroundColor: Colors.green, 
          colorText: Colors.white, 
          duration: Duration(seconds: 5), 
        );
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => EmailConfirmationPage(email: email,)),
        );
      } else {
        Get.snackbar(
          'Error',
          responseBody["message"],
          backgroundColor: Colors.red, 
          colorText: Colors.white, 
          duration: Duration(seconds: 5), 
        );
      }
    } else {
      // Registration failed
      // You can handle the error here
      var errorBody = jsonDecode(response.body);
      var errors = errorBody['errors'];

      if (errors != null && errors is Map) {
        // Iterate over each error and display the message
        errors.forEach((key, value) {
          if (key == 'Username') {
            setState(() {
              usernameErrorText = value[0];
            });
          } else if (key == 'Email') {
            setState(() {
              emailErrorText = value[0];
            });
          } else if (key == 'Password') {
            setState(() {
              passwordErrorText = value[0];
            });
          }
        });
      } else {
        Get.snackbar(
          'Error',
          "Registration failed",
          backgroundColor: Colors.red, 
          colorText: Colors.white, 
          duration: Duration(seconds: 5), 
        );
      }
    }
  } catch (e) {
    Get.snackbar(
      'Error',
      e.toString(),
      backgroundColor: Colors.red,
      colorText: Colors.white,
      duration: Duration(seconds: 5), 
    );
  }
}

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Register'),
      ),
      body: Center(
        child: Container(
          width: 600.0,
          padding: EdgeInsets.all(16.0),
          decoration: BoxDecoration(
            color: Theme.of(context)?.scaffoldBackgroundColor ?? Colors.transparent,
            borderRadius: BorderRadius.circular(10.0),
            boxShadow: [
              BoxShadow(
                color: Colors.grey.withOpacity(0.5),
                spreadRadius: 5,
                blurRadius: 7,
                offset: Offset(0, 3),
              ),
            ],
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Image.asset(
                'images/registration.png',
                width: 150.0,
              ),
              SizedBox(height: 16.0),
              TextField(
                controller: _usernameController,
                style: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                decoration: InputDecoration(
                  labelText: 'Username',
                  labelStyle: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  errorText: usernameErrorText,
                  enabledBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                  focusedBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                ),
              ),
              TextField(
                controller: _emailController,
                style: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                decoration: InputDecoration(
                  labelText: 'Email',
                  labelStyle: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  errorText: emailErrorText,
                  enabledBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                  focusedBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                ),
              ),
              TextField(
                controller: _passwordController,
                obscureText: true,
                style: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                decoration: InputDecoration(
                  labelText: 'Password',
                  labelStyle: TextStyle(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  errorText: passwordErrorText,
                  enabledBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                  focusedBorder: UnderlineInputBorder(
                    borderSide: BorderSide(color: Theme.of(context)?.textTheme.bodyText1?.color ?? Colors.black),
                  ),
                ),
              ),
              SizedBox(height: 16.0),
              ElevatedButton(
                onPressed: () {
                  setState(() {
                    usernameErrorText = null;
                    emailErrorText = null;
                    passwordErrorText = null;
                  });
                  registerUser();
                },
                child: Text('Register'),
                style: ElevatedButton.styleFrom(
                  primary: Theme.of(context)?.primaryColor ?? Colors.blue,
                  onPrimary: Theme.of(context)?.textTheme.button?.color ?? Colors.white,
                ),
              ),
              SizedBox(height: 8.0),
              Text.rich(
                TextSpan(
                  text: 'By registering, you agree to our ',
                  style: TextStyle(
                    fontSize: 12.0,
                    color: Theme.of(context)?.textTheme.caption?.color ?? Colors.grey,
                  ),
                  children: [
                    TextSpan(
                      text: 'Terms and Conditions',
                      style: TextStyle(
                        decoration: TextDecoration.underline,
                        fontWeight: FontWeight.bold,
                        color: Colors.blue,
                      ),
                      recognizer: TapGestureRecognizer()
                        ..onTap = () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => TermsAndConditionsPage()),
                          );
                        },
                    ),
                    TextSpan(
                      text: '.',
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  
}
