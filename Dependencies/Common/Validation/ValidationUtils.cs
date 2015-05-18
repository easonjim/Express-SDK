/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using System.Text;
using System;
using ComLib;


namespace ComLib.ValidationSupport
{

    public class ValidationUtils
    {
        /// <summary>
        /// Check the parameter isValidCondition for validation condition.
        /// If it is not valid, adds the errmessage to the list of errors.
        /// </summary>
        /// <param name="isValidCondition"></param>
        /// <param name="errors"></param>
        /// <param name="errMessage"></param>
        public static bool Validate(bool isError, IList<string> errors, string message )
        {
            if (isError) { errors.Add(message); }
            return !isError;
        }


        /// <summary>
        /// Validates the bool condition and adds the string error
        /// to the error list if the condition is invalid.
        /// </summary>
        /// <param name="isValid">Flag indicating if invalid.</param>
        /// <param name="error">Error message</param>
        /// <param name="errors"><see cref="ValidationResults"/></param>
        /// <returns>True if isError is false, indicating no error.</returns>
        public static bool Validate(bool isError, IErrors errors, string key, string message)
        {
            if (isError) 
            {
                errors.Add(key, message);
            }
            return !isError;
        }



        /// <summary>
        /// Validates the bool condition and adds the string error
        /// to the error list if the condition is invalid.
        /// </summary>
        /// <param name="isValid">Flag indicating if invalid.</param>
        /// <param name="error">Error message</param>
        /// <param name="results"><see cref="ValidationResults"/></param>
        /// <returns>True if isError is false, indicating no error.</returns>
        public static bool Validate(bool isError, IErrors errors, string message)
        {
            if (isError)
            {
                errors.Add(string.Empty, message);
            }
            return !isError;
        }


        /// <summary>
        /// Transfers all the messages from the source to the validation results.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="results"></param>
        public static void TransferMessages(IList<string> messages, IErrors errors)
        {
            foreach (string message in messages)
            {
                errors.Add(string.Empty, message);
            }
        }
        

        /// <summary>
        /// Valdiates all the validation rules in the list.
        /// </summary>
        /// <param name="validationRules">List of validation rules to validate</param>
        /// <param name="validationErrors">List of validation results to populate.
        /// This list is populated with the errors from the validation rules.</param>
        /// <returns>True if all rules passed, false otherwise.</returns>
        public static bool Validate(IList<IValidator> validators, IValidationResults destinationResults)
        {
            if (validators == null || validators.Count == 0)
                return true;

            // Get the the initial error count;		
            int initialErrorCount = destinationResults.Count;

            // Iterate through all the validation rules and validate them.
            foreach(IValidator validator in validators)
            {
                validator.Validate(destinationResults);
            }

            // Determine validity if errors were added to collection.
            return initialErrorCount == destinationResults.Count;
        }


        /// <summary>
        /// Validates the rule and returns a boolMessage.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static BoolMessage Validate(IValidator validator)
        {
            IValidationResults results = validator.Validate() as IValidationResults;
            
            // Empty message if Successful.
            if (results.IsValid) return new BoolMessage(true, string.Empty);

            // Error            
            string multiLineError = results.Message();
            return new BoolMessage(false, multiLineError);
        }


        /// <summary>
        /// Validates the rule and returns a boolMessage.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool ValidateAndCollect(IValidator validator, IValidationResults results)
        {
            IValidationResults validationResults = validator.Validate(results);
            return validationResults.IsValid;
        }
    }
}