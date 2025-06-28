# app.py
# This is the main entry point for our Flask backend application.

# Import necessary modules from Flask for creating the web server and handling requests.
from flask import Flask, request, jsonify

# Import the translation service we will create later.
# This keeps our machine learning logic separate from the web server logic.
# For now, we'll assume it has a function called `translate_sign_language`.
# from ml_service.translator import translate_sign_language

# --- Placeholder for ML Service ---
# Since we haven't built the ML service yet, let's create a placeholder function.
# This function mimics what our real translator will do.
# TODO: Replace this with the actual import once ml_service/translator.py is ready.
def translate_sign_language(data):
    """
    Placeholder function to simulate sign language translation.
    In the real version, this will process the data with a machine learning model.
    """
    print(f"Received data for translation: {data}")
    # For now, just return a dummy translation.
    # The real function will return the model's output.
    return "Hello, from the backend!"
# --- End of Placeholder ---


# Initialize the Flask application.
# '__name__' is a special Python variable that gives the script's name.
# Flask uses this to know where to look for resources.
app = Flask(__name__)

# Define a route for our API. This is the URL that Unity will send data to.
# We specify methods=['POST'] because we expect to receive data, not just serve a page.
@app.route('/translate', methods=['POST'])
def handle_translation():
    """
    This function handles the incoming requests to the /translate endpoint.
    """
    try:
        # Get the JSON data sent from the Unity application.
        # request.get_json() parses the incoming request body as JSON.
        incoming_data = request.get_json()

        if not incoming_data:
            # If no JSON data was sent, return an error response.
            return jsonify({"error": "No data provided in the request."}), 400

        # --- Call the Machine Learning Service ---
        # Pass the received data to our translation function.
        translated_text = translate_sign_language(incoming_data)

        # --- Send the Response ---
        # Create a successful JSON response containing the translated text.
        # The HTTP status code 200 means 'OK'.
        return jsonify({
            "status": "success",
            "translation": translated_text
        }), 200

    except Exception as e:
        # If any other error occurs during the process, log it and return a generic error.
        print(f"An error occurred: {e}")
        return jsonify({"error": "An internal server error occurred."}), 500

# This is a standard Python construct.
# The code inside this block will only run if the script is executed directly
# (e.g., by running 'python app.py' in the terminal).
if __name__ == '__main__':
    # Start the Flask development server.
    # host='0.0.0.0' makes the server accessible from any IP address, including from Unity.
    # port=5000 is the standard port for Flask, but you can change it.
    # debug=True enables debug mode, which provides helpful error messages and auto-reloads the server when you save changes.
    app.run(host='0.0.0.0', port=1080, debug=True)
