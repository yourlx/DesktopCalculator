#ifndef CALC_MODEL_TOKEN_H_
#define CALC_MODEL_TOKEN_H_

namespace model {

enum class TokenType { kNone, kBracket, kNumber, kOperator, kFunction };

class Token {
 public:
  Token() = default;
  Token(const Token &other) = delete;
  Token(Token &&other) = delete;
  Token &operator=(const Token &other) = delete;
  Token &operator=(Token &&other) = delete;
  explicit Token(TokenType type);
  virtual ~Token() = 0;

  TokenType GetType();

 private:
  TokenType type_ = TokenType::kNone;
};

class Bracket final : public Token {
 public:
  explicit Bracket(bool opening);
  bool IsOpening() const;

 private:
  bool opening_ = true;
};

enum class OperatorType { kAdd, kSub, kMul, kDiv, kPow, kMod };

class Operator final : public Token {
 public:
  explicit Operator(OperatorType operator_type);
  OperatorType GetOperatorType();

 private:
  OperatorType operator_type_;
};

enum class FunctionType {
  kCos,
  kSin,
  kTan,
  kAcos,
  kAsin,
  kAtan,
  kSqrt,
  kLn,
  kLog
};

class Function final : public Token {
 public:
  explicit Function(FunctionType function_type);
  FunctionType GetFunctionType();

 private:
  FunctionType function_type_;
};

class Number final : public Token {
 public:
  explicit Number(bool variable, double value = 0);
  bool IsVariable() const;
  double GetValue() const;

 private:
  bool variable_;
  double value_;
};

}  // namespace model

#endif  // CALC_MODEL_TOKEN_H_
