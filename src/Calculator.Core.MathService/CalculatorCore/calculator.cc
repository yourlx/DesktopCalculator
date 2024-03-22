#include "model_maths.h"

extern "C" {
model::Maths *CreateInstance() { return new model::Maths(); }

void FreeInstance(model::Maths *ptr) { delete ptr; }

bool CheckValid(model::Maths *ptr, const char *expression) {
  ptr->SetExpression(expression);
  ptr->PrepareExpression();
  return ptr->ValidateExpression();
}

void ConvertToPolish(model::Maths *ptr) { ptr->ConvertToPolish(); }

double Calculate(model::Maths *ptr, double x = 0) {
  return ptr->CalculateExpression(x);
}
}
