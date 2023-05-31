import 'dart:convert';
import 'package:http/http.dart' as http;

class ApiService {
  static const String baseUrl = 'http://localhost:5000/api';

  Future<http.Response> post(String endpoint, dynamic data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/$endpoint'), 
      body: jsonEncode(data), 
      headers: {'Content-Type': 'application/json'}, 
    );
    return response;
  }
}