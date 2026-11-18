import uvicorn
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List
from transformers import AutoTokenizer
import ctranslate2

app = FastAPI(title="Simple Translate API", description="")

MODEL_PATH = "C:\\Users\\heero\\Source\\SimpleTranslate\\python\\model\\"

try:
    tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)
    translator = ctranslate2.Translator(MODEL_PATH, device="cuda")
except Exception as e:
    print(f"Model can't be load：{e}")
    exit(1)

class TranslationRequest(BaseModel):
    text: str
    source_lang: str = "zho_Hans"
    target_lang: str = "eng_Latn"

class TranslationResponse(BaseModel):
    translated_text: str
    source_lang: str
    target_lang: str

@app.post("/translate", response_model=TranslationResponse)
async def translate(request: TranslationRequest):
    try:
        tokenizer.src_lang = request.source_lang
        source_tokens = tokenizer.convert_ids_to_tokens(tokenizer.encode(request.text))
        results = translator.translate_batch(
            [source_tokens],
            target_prefix=[[request.target_lang]]
        )
        target_tokens = results[0].hypotheses[0]
        if target_tokens and target_tokens[0] == request.target_lang:
            target_tokens = target_tokens[1:]
        translated_text = tokenizer.decode(tokenizer.convert_tokens_to_ids(target_tokens))
        
        return TranslationResponse(
            translated_text=translated_text,
            source_lang=request.source_lang,
            target_lang=request.target_lang
        )
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error: {str(e)}")

# 启动服务
if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=54413)
