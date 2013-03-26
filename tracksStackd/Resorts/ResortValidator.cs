using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tracksStackd.Resorts
{
    public class ResortValidator : AbstractValidator<Resort>
    {
        public ResortValidator()
        {
            RuleFor(resort => resort.Name).NotEmpty();
        }
    }

    public class TrackValidator : AbstractValidator<Track>
    {
        public TrackValidator()
        {
            RuleFor(track => track.Name).NotEmpty();
        }
    }
}
