import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'services/ApiService.dart';
import 'dart:convert';

class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  TextEditingController _usernameController = TextEditingController();
  TextEditingController _passwordController = TextEditingController();

 ApiService _apiService = ApiService();

  String? usernameErrorText;
  String? passwordErrorText;

  Future<void> loginUser() async {
    var endpoint = 'Authentication/login';
    var data = {
      'username': _usernameController.text,
      'password': _passwordController.text,
    };

    try {
      var response = await _apiService.post(endpoint, data);

      if (response.statusCode == 200) {
          var responseBody = jsonDecode(response.body);
          if (responseBody["status"]) {
            Get.snackbar(
              'Success',
              responseBody["message"],
              backgroundColor: Colors.green, 
              colorText: Colors.white, 
              duration: Duration(seconds: 5), 
            );
            Navigator.pushNamed(context, '/home'); // Navigate to home page
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
        // Login failed
        // You can handle the error here
        var errorBody = jsonDecode(response.body);
        var error = errorBody['error'];
        setState(() {
          if (error == 'Invalid credentials') {
            passwordErrorText = 'Invalid username or password';
          } else {
            passwordErrorText = 'Login failed';
          }
        });
      }
    } catch (e) {
      Get.snackbar('Error', e.toString());
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Login'),
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
                'images/login.png',
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
                    passwordErrorText = null;
                  });
                  loginUser();
                },
                child: Text('Login'),
                style: ElevatedButton.styleFrom(
                  primary: Theme.of(context)?.primaryColor ?? Colors.blue,
                  onPrimary: Theme.of(context)?.textTheme.button?.color ?? Colors.white,
                ),
              ),
              SizedBox(height: 8.0),
              Text(
                'Forgot password?',
                style: TextStyle(
                  fontSize: 12.0,
                  color: Theme.of(context)?.textTheme.caption?.color ?? Colors.grey,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
