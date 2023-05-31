import 'package:flutter/material.dart';
import 'package:get/get.dart';

class HomePage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Home'),
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
              TweenAnimationBuilder(
                tween: Tween<double>(begin: 0, end: 1),
                duration: Duration(seconds: 1),
                builder: (BuildContext context, double value, Widget? child) {
                  return Opacity(
                    opacity: value,
                    child: child,
                  );
                },
                child: Image.asset(
                  'images/voxella_logo.png',
                  width: 800.0,
                ),
              ),
              SizedBox(height: 16.0),
              Text(
                'Welcome to Voxella',
                style: TextStyle(
                  fontSize: 24.0,
                  fontWeight: FontWeight.bold,
                  shadows: [
                    Shadow(
                      color: Colors.grey.withOpacity(0.5),
                      blurRadius: 4,
                      offset: Offset(2, 2),
                    ),
                  ],
                ),
              ),
              SizedBox(height: 16.0),
              ElevatedButton(
                onPressed: () {
                  Get.toNamed('/login');
                },
                child: Text('Login'),
                style: ElevatedButton.styleFrom(
                  primary: Theme.of(context)?.primaryColor ?? Colors.blue,
                  onPrimary: Theme.of(context)?.textTheme.button?.color ?? Colors.white,
                  elevation: 3,
                  padding: EdgeInsets.symmetric(horizontal: 32.0, vertical: 16.0),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(30.0),
                  ),
                ),
              ),
              SizedBox(height: 8.0),
              Text(
                'Don\'t have an account?',
                style: TextStyle(
                  fontSize: 12.0,
                  color: Theme.of(context)?.textTheme.caption?.color ?? Colors.grey,
                ),
              ),
              SizedBox(height: 8.0),
              ElevatedButton(
                onPressed: () {
                  Get.toNamed('/register');
                },
                child: Text('Register'),
                style: ElevatedButton.styleFrom(
                  primary: Theme.of(context)?.primaryColor ?? Colors.blue,
                  onPrimary: Theme.of(context)?.textTheme.button?.color ?? Colors.white,
                  elevation: 3,
                  padding: EdgeInsets.symmetric(horizontal: 32.0, vertical: 16.0),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(30.0),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
