import Vue from 'vue'
import {
  extend,
  ValidationObserver,
  ValidationProvider,
} from 'vee-validate'
import {
  email,
  max,
  min,
  numeric,
  required,
} from 'vee-validate/dist/rules'

extend('email', email)
extend('max', max)
extend('min', min)
extend('numeric', numeric)
extend('required', required)
let errorMassage = 'min length 8 chars, and must include 1 lower-case, upper-case, number and special character (@$!%*?&)';
extend('customPassword', {
  message: field => `The ${field}  ${errorMassage}`,
  validate: value => {
    const validChars = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    const isValidChars = validChars.test(value);
    if (isValidChars) {
      return true
    } else {
      errorMassage = 'min length 8 chars, and must include 1 lower-case, upper-case, number and special character (@$!%*?&)';
      return false
    }
  }

})
Vue.component('validation-provider', ValidationProvider)
Vue.component('validation-observer', ValidationObserver)
