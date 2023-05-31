import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'dart:convert';
import 'services/ApiService.dart';

class EmailConfirmationPage extends StatefulWidget {
  String? email;
  EmailConfirmationPage({this.email});
  @override
  _EmailConfirmationPageState createState() => _EmailConfirmationPageState(email!);
}

class _EmailConfirmationPageState extends State<EmailConfirmationPage> {
  String email;
  _EmailConfirmationPageState(this.email);
  TextEditingController _confirmationCodeController = TextEditingController();
  ApiService _apiService = ApiService();

  Future<void> confirmEmail() async {
    String confirmationCode = _confirmationCodeController.text;
    var endpoint = 'Authentication/email-confirmation';
    var data = {
      'Email': email,
      'EmailConfirmationCode': confirmationCode,
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
            duration: Duration(seconds: 4),
          );
          Navigator.pushNamedAndRemoveUntil(context, '/home', (route) => false);
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
        // Confirmation failed
        // You can handle the error here
        var errorBody = jsonDecode(response.body);
        var message = errorBody['message'];

        showDialog(
          context: context,
          builder: (context) => AlertDialog(
            title: Text('Error'),
            content: Text(message),
            actions: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: Text('OK'),
              ),
            ],
          ),
        );
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
        title: Text('Email Confirmation'),
      ),
      body: Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              Icons.email_outlined,
              size: 100,
              color: Colors.blue,
            ),
            SizedBox(height: 16.0),
            Text(
              'Please enter the confirmation code sent to your email:',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 16.0),
            ),
            SizedBox(height: 16.0),
            Container(
              width: 200,
              child: TextField(
                controller: _confirmationCodeController,
                keyboardType: TextInputType.number,
                textAlign: TextAlign.center,
                decoration: InputDecoration(
                  hintText: 'Confirmation Code',
                ),
              ),
            ),
            SizedBox(height: 16.0),
            ElevatedButton(
              onPressed: confirmEmail,
              child: Text('Continue'),
            ),
          ],
        ),
      ),
    );
  }
}
