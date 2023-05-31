import 'package:flutter/material.dart';

class TermsAndConditionsPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Terms and Conditions'),
      ),
      body: Center(
        child: Container(
          padding: EdgeInsets.all(16.0),
          constraints: BoxConstraints(maxWidth: 600.0),
          child: Card(
            elevation: 3,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(8.0),
            ),
            child: Padding(
              padding: EdgeInsets.all(16.0),
              child: ListView(
                children: [
                  Text(
                    'Terms and Conditions',
                    style: TextStyle(
                      fontSize: 24.0,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  SizedBox(height: 16.0),
                  Text(
                    'Welcome to Voxella!',
                    style: TextStyle(
                      fontSize: 18.0,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    'Please read these terms and conditions carefully before using the Voxella application.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 16.0),
                  Text(
                    'By using the Voxella application, you agree to the following terms and conditions:',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 16.0),
                  Text(
                    '1. You must not use the Voxella application for any illegal or unauthorized purposes.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    '2. You are solely responsible for the content you upload or share through the Voxella application.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    '3. Voxella reserves the right to suspend or terminate your access to the application at any time.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    '4. The translations and dubbing provided by Voxella are generated using artificial intelligence and may not be 100% accurate.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    '5. Voxella is not responsible for any inaccuracies or errors in the translated or dubbed content.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                  SizedBox(height: 8.0),
                  Text(
                    '6. By using the Voxella application, you agree to receive occasional promotional messages and updates.',
                    style: TextStyle(fontSize: 16.0),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
