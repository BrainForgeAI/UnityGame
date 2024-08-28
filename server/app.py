import os
import sys
import logging
from flask import Flask, render_template, request, jsonify, Response

# Add the parent directory to the Python path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
from QuestionGenerator.QuestionGenerator import QuestionGenerator
from QuestionGenerator.load_syllabus import load_syllabus

app = Flask(__name__)

# Configure logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

qg = None
current_question = {}

@app.route('/load_syllabus', methods=['POST'])
def upload_syllabus() -> Response:
    logger.info("Received request for /load_syllabus")
    if 'file' not in request.files:
        logger.warning("No file part in the request")
        return jsonify({'error': 'No file part'}), 400
    
    file = request.files['file']
    logger.info(f"File received: {file.filename}")
    
    if file.filename == '':
        logger.warning("No selected file")
        return jsonify({'error': 'No selected file'}), 400
    
    if file:
        filename = file.filename
        file_path = os.path.join('uploads', filename)
        os.makedirs('uploads', exist_ok=True)
        file.save(file_path)
        logger.info(f"File saved to: {file_path}")
        
        try:
            global qg
            logger.info("Attempting to load syllabus...")
            _, syllabus = load_syllabus(gemini_model=QuestionGenerator.model, path_to_file=file_path)
            logger.info("Syllabus loaded successfully")
            qg = QuestionGenerator(syllabus=syllabus)
            return jsonify({'message': 'Syllabus loaded successfully'}), 200
        except Exception as e:
            logger.error(f"Error loading syllabus: {str(e)}", exc_info=True)
            return jsonify({'error': str(e)}), 500

@app.route('/')
def index() -> str:
    """
    Renders index.html.
    """
    return render_template('index.html')

@app.route('/get_question', methods=['GET'])
def get_question() -> Response:
    """
    Get request for question prompt.
    
    :return jsonify(current_question):
        JSON Response object.
    """
    global current_question, qg
    if qg is None:
        logger.warning("Attempt to get question when syllabus is not loaded")
        return jsonify({'error': 'Syllabus not loaded'}), 400
    _, _, current_question = qg.generate_response_questions()
    
    return jsonify(current_question)

@app.route('/submit_answer', methods=['POST'])
def submit_answer() -> Response:
    """
    Post request for question prompt.
    
    :return jsonify(current_question):
        JSON Response object.
    """
    global current_question, qg
    if qg is None:
        logger.warning("Attempt to submit answer when syllabus is not loaded")
        return jsonify({'error': 'Syllabus not loaded'}), 400
    answer = request.form['answer']
    _, _, current_question = qg.generate_response_questions(answer=answer)
    
    if "IAMDONE" in current_question['question']:
        logger.info("Question generation completed")
        return jsonify({'question': "Question generation completed.", 'completed': True})
    
    return jsonify(current_question)

if __name__ == '__main__':
    app.run(debug=True, port=5001)
