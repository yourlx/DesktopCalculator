#include "model_token.h"

namespace model {

Token::Token(TokenType type) { type_ = type; }

Token::~Token() = default;

TokenType Token::GetType() { return type_; }

Bracket::Bracket(bool opening) : Token(TokenType::kBracket) {
  opening_ = opening;
}

bool Bracket::IsOpening() const { return opening_; }

Operator::Operator(OperatorType operator_type) : Token(TokenType::kOperator) {
  operator_type_ = operator_type;
}

OperatorType Operator::GetOperatorType() { return operator_type_; }

Function::Function(FunctionType function_type) : Token(TokenType::kFunction) {
  function_type_ = function_type;
}

FunctionType Function::GetFunctionType() { return function_type_; }

Number::Number(bool variable, double value) : Token(TokenType::kNumber) {
  variable_ = variable;
  value_ = value;
}

bool Number::IsVariable() const { return variable_; }

double Number::GetValue() const { return value_; }

}  // namespace model
