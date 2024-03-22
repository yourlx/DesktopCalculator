#include "model_maths.h"

namespace model {

Maths::~Maths() { Clear(); }

void Maths::SetExpression(const char *expression) {
  Clear();
  expression_ = std::string(expression);
}

std::string Maths::GetExpression() { return expression_; }

void Maths::PrepareExpression() {
  RemoveSpaces();
  ReplaceMod();
  ReplaceUnary();
  TokenizeExpression();
}

bool Maths::ValidateExpression() {
  if (expression_.length() == 0) return false;
  if (!tokenized_) return false;
  if (!BracketsCountCheck()) return false;
  if (!TokenValidator()) return false;
  if (expression_.find("0x") != std::string::npos) return false;
  //  ConvertToPolish();
  return true;
}

double Maths::CalculateExpression(double x) { return CalculatePolish(x); }

void Maths::Clear() {
  for (auto &elem : tokens_) {
    delete elem;
  }
  tokens_.clear();
  polish_.clear();
}

bool Maths::HigherOperatorPriority(list_iterator token1, list_iterator token2) {
  int token_priority = 0;
  for (auto &elem : priorities) {
    if (dynamic_cast<Operator *>(*token1)->GetOperatorType() == elem.first) {
      token_priority -= elem.second;
    }
    if (dynamic_cast<Operator *>(*token2)->GetOperatorType() == elem.first) {
      token_priority += elem.second;
    }
  }
  if (token_priority == 0 &&
      dynamic_cast<Operator *>(*token1)->GetOperatorType() ==
          OperatorType::kPow) {
    return false;
  }
  return token_priority <= 0;
}

bool Maths::IsTokenNumberVariable(list_iterator token) {
  return dynamic_cast<Number *>(*token)->IsVariable();
}

double Maths::GetTokenNumberValue(list_iterator token) {
  return dynamic_cast<Number *>(*token)->GetValue();
}

double Maths::ApplyFunction(list_iterator token, std::vector<double> &numbers) {
  double result = 0;
  if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
      FunctionType::kCos) {
    result = cos(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kSin) {
    result = sin(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kTan) {
    result = tan(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kAcos) {
    result = acos(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kAsin) {
    result = asin(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kAtan) {
    result = atan(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kSqrt) {
    result = sqrt(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kLn) {
    result = log(numbers.back());
  } else if (dynamic_cast<Function *>(*token)->GetFunctionType() ==
             FunctionType::kLog) {
    result = log10(numbers.back());
  }
  numbers.pop_back();
  return result;
}

double Maths::ApplyOperator(list_iterator token, std::vector<double> &numbers) {
  double result = 0;
  double second_operand = numbers.back();
  numbers.pop_back();
  if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
      OperatorType::kAdd) {
    result = numbers.back() + second_operand;
  } else if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
             OperatorType::kSub) {
    result = numbers.back() - second_operand;
  } else if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
             OperatorType::kMul) {
    result = numbers.back() * second_operand;
  } else if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
             OperatorType::kDiv) {
    result = numbers.back() / second_operand;
  } else if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
             OperatorType::kPow) {
    result = pow(numbers.back(), second_operand);
  } else if (dynamic_cast<Operator *>(*token)->GetOperatorType() ==
             OperatorType::kMod) {
    result = fmod(numbers.back(), second_operand);
  }
  numbers.pop_back();
  return result;
}

void Maths::ConvertToPolish() {
  auto current = tokens_.begin();
  std::list<Token *> temp;
  while (current != tokens_.end()) {
    if (IsTokenNumber(current)) {
      polish_.push_back(*current);
    } else if (IsTokenOpeningBracket(current)) {
      temp.push_back(*current);
    } else if (IsTokenClosingBracket(current)) {
      while (!IsTokenOpeningBracket(--temp.end())) {
        polish_.push_back(temp.back());
        temp.pop_back();
      }
      temp.pop_back();
    } else if (IsTokenOperator(current)) {
      while (!temp.empty() &&
             ((IsTokenOperator(--temp.end()) &&
               HigherOperatorPriority(--temp.end(), current)) ||
              IsTokenFunction(--temp.end()))) {
        polish_.push_back(temp.back());
        temp.pop_back();
      }
      temp.push_back(*current);
    } else if (IsTokenFunction(current)) {
      temp.push_back(*current);
    }
    ++current;
  }
  while (!temp.empty()) {
    polish_.push_back(temp.back());
    temp.pop_back();
  }
}

double Maths::CalculatePolish(double x) {
  std::vector<double> numbers;
  auto current = polish_.begin();
  while (current != polish_.end()) {
    if (IsTokenNumber(current)) {
      if (IsTokenNumberVariable(current)) {
        numbers.push_back(x);
      } else {
        numbers.push_back(GetTokenNumberValue(current));
      }
    } else if (IsTokenFunction(current)) {
      numbers.push_back(ApplyFunction(current, numbers));
    } else if (IsTokenOperator(current)) {
      numbers.push_back(ApplyOperator(current, numbers));
    }
    ++current;
  }
  return numbers.back();
}

bool Maths::SubstringExists(const std::string &substring, size_type i) {
  size_type j = 0;
  while (j < substring.length() && i < expression_.length()) {
    if (expression_[i] != substring[j]) {
      return false;
    }
    ++i;
    ++j;
  }
  return true;
}

bool Maths::IsFunction(const size_type i) {
  for (auto &elem : functions) {
    if (SubstringExists(elem.first, i)) {
      return true;
    }
  }
  return false;
}

void Maths::Replacer(const std::string &from, const std::string &to) {
  while (expression_.find(from) != std::string::npos) {
    expression_.replace(expression_.find(from), from.length(), to);
  }
}

void Maths::RemoveSpaces() {
  expression_.erase(std::remove(expression_.begin(), expression_.end(), ' '),
                    expression_.end());
}

void Maths::ReplaceMod() { Replacer("mod", "%"); }

void Maths::ReplaceUnary() {
  for (size_type i = 0; i < expression_.length(); ++i) {
    if ((i == 0 || expression_[i - 1] == '(') &&
        (isdigit(expression_[i + 1]) || expression_[i + 1] == '.' ||
         expression_[i + 1] == 'x' || expression_[i + 1] == '(' ||
         IsFunction(i + 1))) {
      if (expression_[i] == '+') {
        expression_[i] = ' ';
      } else if (expression_[i] == '-') {
        expression_[i] = '~';
      }
    }
  }
  Replacer("~", "0-");
  RemoveSpaces();
}

bool Maths::BracketsCountCheck() {
  int result = 0;
  for (char i : expression_) {
    if (i == '(') {
      result++;
    } else if (i == ')') {
      result--;
    }
    if (result < 0) return false;
  }
  return result == 0;
}

void Maths::TokenizeExpression() {
  tokenized_ = true;
  for (size_type i = 0; i < expression_.length(); ++i) {
    if (!BracketToTokensList(i) && !OperatorToTokensList(i) &&
        !FunctionToTokensList(i) && !NumberToTokensList(i)) {
      tokenized_ = false;
    }
  }
}

bool Maths::BracketToTokensList(size_type &i) {
  std::string brackets = "()";
  if (brackets.find(expression_[i]) == std::string::npos) {
    return false;
  }
  if (expression_[i] == '(') {
    tokens_.push_back(new Bracket(true));
  } else if (expression_[i] == ')') {
    tokens_.push_back(new Bracket(false));
  }
  return true;
}

bool Maths::OperatorToTokensList(size_type &i) {
  std::string operators = "+-*/^%";
  if (operators.find(expression_[i]) == std::string::npos) {
    return false;
  }
  if (expression_[i] == '+') {
    tokens_.push_back(new Operator(OperatorType::kAdd));
  } else if (expression_[i] == '-') {
    tokens_.push_back(new Operator(OperatorType::kSub));
  } else if (expression_[i] == '*') {
    tokens_.push_back(new Operator(OperatorType::kMul));
  } else if (expression_[i] == '/') {
    tokens_.push_back(new Operator(OperatorType::kDiv));
  } else if (expression_[i] == '^') {
    tokens_.push_back(new Operator(OperatorType::kPow));
  } else if (expression_[i] == '%') {
    tokens_.push_back(new Operator(OperatorType::kMod));
  }
  return true;
}

bool Maths::FunctionToTokensList(size_type &i) {
  for (auto &elem : functions) {
    if (SubstringExists(elem.first, i)) {
      tokens_.push_back(new Function(elem.second));
      i += elem.first.length() - 1;
      return true;
    }
  }
  return false;
}

bool Maths::NumberToTokensList(size_type &i) {
  try {
    if (expression_[i] == 'x') {
      tokens_.push_back(new Number(true));
    } else {
      size_type j = 0;
      double value = std::stod(expression_.substr(i), &j);
      i += j - 1;
      tokens_.push_back(new Number(false, value));
    }
  } catch (...) {
    return false;
  }
  return true;
}

bool Maths::IsTokenOpeningBracket(list_iterator token) {
  if ((*token)->GetType() == TokenType::kBracket &&
      dynamic_cast<Bracket *>(*token)->IsOpening()) {
    return true;
  }
  return false;
}

bool Maths::IsTokenClosingBracket(list_iterator token) {
  if ((*token)->GetType() == TokenType::kBracket &&
      !(dynamic_cast<Bracket *>(*token)->IsOpening())) {
    return true;
  }
  return false;
}

bool Maths::IsTokenOperator(list_iterator token) {
  if ((*token)->GetType() == TokenType::kOperator) {
    return true;
  }
  return false;
}

bool Maths::IsTokenFunction(list_iterator token) {
  if ((*token)->GetType() == TokenType::kFunction) {
    return true;
  }
  return false;
}

bool Maths::IsTokenNumber(list_iterator token) {
  if ((*token)->GetType() == TokenType::kNumber) {
    return true;
  }
  return false;
}

bool Maths::FirstTokenValid() {
  if (IsTokenOpeningBracket(tokens_.begin()) ||
      IsTokenFunction(tokens_.begin()) || IsTokenNumber(tokens_.begin())) {
    return true;
  }
  return false;
}

bool Maths::LastTokenValid() {
  if (IsTokenClosingBracket(--tokens_.end()) ||
      IsTokenNumber(--tokens_.end())) {
    return true;
  }
  return false;
}

bool Maths::TokenValidator() {
  if (tokens_.size() == 1) {
    if ((*tokens_.begin())->GetType() == TokenType::kNumber) {
      return true;
    } else {
      return false;
    }
  }
  if (!FirstTokenValid()) return false;
  if (!LastTokenValid()) return false;
  auto current = tokens_.begin();
  auto next = ++tokens_.begin();
  while (next != tokens_.end()) {
    if (!(IsTokenFunction(current) && IsTokenOpeningBracket(next)) &&
        !(IsTokenNumber(current) &&
          (IsTokenOperator(next) || IsTokenClosingBracket(next))) &&
        !(IsTokenOperator(current) &&
          (IsTokenOpeningBracket(next) || IsTokenNumber(next) ||
           IsTokenFunction(next))) &&
        !(IsTokenOpeningBracket(current) &&
          (IsTokenOpeningBracket(next) || IsTokenNumber(next) ||
           IsTokenFunction(next))) &&
        !(IsTokenClosingBracket(current) &&
          (IsTokenOperator(next) || IsTokenClosingBracket(next)))) {
      return false;
    }
    ++current;
    ++next;
  }
  return true;
}

}  // namespace model
