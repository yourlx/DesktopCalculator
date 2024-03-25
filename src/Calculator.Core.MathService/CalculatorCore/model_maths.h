#ifndef CALC_MODEL_MATHS_H_
#define CALC_MODEL_MATHS_H_

#include <algorithm>
#include <cmath>
#include <iostream>
#include <list>
#include <map>
#include <queue>
#include <stack>

#include "model_token.h"

namespace model {

inline std::map<std::string, FunctionType> functions{
    {"cos", FunctionType::kCos},   {"sin", FunctionType::kSin},
    {"tan", FunctionType::kTan},   {"acos", FunctionType::kAcos},
    {"asin", FunctionType::kAsin}, {"atan", FunctionType::kAtan},
    {"sqrt", FunctionType::kSqrt}, {"ln", FunctionType::kLn},
    {"log", FunctionType::kLog}};

inline std::map<OperatorType, int> priorities{
    {OperatorType::kAdd, 1}, {OperatorType::kSub, 1}, {OperatorType::kMul, 2},
    {OperatorType::kDiv, 2}, {OperatorType::kPow, 3}, {OperatorType::kMod, 2}};

class Maths final {
 public:
  Maths() = default;
  Maths(const Maths &other) = delete;
  Maths(Maths &&other) = delete;
  Maths &operator=(const Maths &other) = delete;
  Maths &operator=(Maths &&other) = delete;
  ~Maths();

  void SetExpression(const char *expression);
  std::string GetExpression();

  void PrepareExpression();

  bool ValidateExpression();

  double CalculateExpression(double x = 0);

  void ConvertToPolish();

  void Clear();

 private:
  bool tokenized_ = false;
  std::string expression_;
  std::list<Token *> tokens_;
  std::list<Token *> polish_;
  using list_iterator = std::list<Token *>::iterator;
  using size_type = std::string::size_type;

  static bool HigherOperatorPriority(list_iterator token1,
                                     list_iterator token2);
  static bool IsTokenNumberVariable(list_iterator token);
  static double GetTokenNumberValue(list_iterator token);

  static double ApplyFunction(list_iterator token,
                              std::vector<double> &numbers);
  static double ApplyOperator(list_iterator token,
                              std::vector<double> &numbers);

  double CalculatePolish(double x);

  bool SubstringExists(const std::string &substring, size_type i);

  bool IsFunction(const size_type i);

  void Replacer(const std::string &from, const std::string &to);

  void RemoveSpaces();
  void ReplaceMod();
  void ReplaceUnary();

  bool BracketsCountCheck();

  void TokenizeExpression();

  bool BracketToTokensList(size_type &i);
  bool OperatorToTokensList(size_type &i);
  bool FunctionToTokensList(size_type &i);
  bool NumberToTokensList(size_type &i);

  static bool IsTokenOpeningBracket(list_iterator token);
  static bool IsTokenClosingBracket(list_iterator token);
  static bool IsTokenOperator(list_iterator token);
  static bool IsTokenFunction(list_iterator token);
  static bool IsTokenNumber(list_iterator token);

  bool FirstTokenValid();
  bool LastTokenValid();

  bool TokenValidator();
};

}  // namespace model

#endif  // CALC_MODEL_MATHS_H_
