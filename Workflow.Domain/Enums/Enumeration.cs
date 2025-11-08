namespace Workflow.Domain.Enums
{
    public class Enumeration
    {
        public enum ApplicationRoleDescriptionEnum
        {
            Manager = 1,
            Finance = 2,
            Employee = 3,
            System = 4
        }

        public enum ActionTypeEnum
        {
            Initial = 1,
            Input = 2,
            Output = 3,
            ApprovalReject = 4,
            End = 5
        }

        public enum ProcessStatusEnum
        {
            Pending = 1,
            Active = 2,
            Completed = 3
        }

        public enum ResponseCodeEnum
        {
            Success = 200,
            BadRequest = 400,
            UnAuthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            MethodNotAllowed = 405,
            Duplicate = 409,
            NextStepNameOneNullAndOneOnlyViolated = 460,
            NextStepReferToTheSameStep = 461,
            NamesAreSystemStepsNames = 462,
            NextStepNotReferToStepsNames = 463,
            CantApplyStepAndPreviousSteps = 470,
            ShouldEnterInitiatorViolation = 471,
            NotSuitableInitaitor = 472,
            ExternalAPINotAllowed = 473,
            InvalidCredentials = 480,
            InternalServerError = 500
        }
    }
}