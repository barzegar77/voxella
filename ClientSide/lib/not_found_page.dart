import 'package:flutter/material.dart';

class NotFoundPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Page Not Found'),
      ),
      body: Container(
        width: double.infinity,
        height: double.infinity,
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [Colors.blue.shade400, Colors.blue.shade900],
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Icon(
              Icons.error_outline,
              size: 80,
              color: Colors.white,
            ),
            SizedBox(height: 16),
            Text(
              '404 - Page Not Found',
              style: TextStyle(fontSize: 24, color: Colors.white),
            ),
            SizedBox(height: 16),
            ElevatedButton(
              onPressed: () {
                // Navigate to the home page or any other page
              },
              child: Text('Go Back'),
              style: ElevatedButton.styleFrom(
                primary: Colors.white,
                onPrimary: Colors.blue,
                padding: EdgeInsets.symmetric(horizontal: 24, vertical: 12),
                textStyle: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(30),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
