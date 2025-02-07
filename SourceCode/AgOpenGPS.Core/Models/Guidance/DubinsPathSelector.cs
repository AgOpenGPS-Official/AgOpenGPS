using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AgOpenGPS.Core.Models
{
    public class DubinsPathSelector
    {
        private readonly DubinsPathConstraints _constraints;
        private List<DubinsPath> _paths = new List<DubinsPath>();

        public DubinsPathSelector(DubinsPathConstraints constraints)
        {
            _constraints = constraints;
            GeoCircle startRightCircle = DubinsPath.ComputeCircle(constraints.StartConstraint, constraints.RadiusConstraint, TurnType.Right);
            GeoCircle startLeftCircle = DubinsPath.ComputeCircle(constraints.StartConstraint, constraints.RadiusConstraint, TurnType.Left);
            GeoCircle goalRightCircle = DubinsPath.ComputeCircle(constraints.GoalConstraint, constraints.RadiusConstraint, TurnType.Right);
            GeoCircle goalLeftCircle = DubinsPath.ComputeCircle(constraints.GoalConstraint, constraints.RadiusConstraint, TurnType.Left);

            // 2 x OuterDubinsPath
            if (RsrDubinsPath.PathIsPossible(startRightCircle, goalRightCircle))
            {
                _paths.Add(new RsrDubinsPath(_constraints));
            }
            if (LslDubinsPath.PathIsPossible(startLeftCircle, goalLeftCircle))
            {
                _paths.Add(new LslDubinsPath(_constraints));
            }
            // 2 x CurvedDubinsPath
            if (LrlDubinsPath.PathIsPossible(startLeftCircle, goalLeftCircle))
            {
                _paths.Add(new LrlDubinsPath(_constraints));
            }
            if (RlrDubinsPath.PathIsPossible(startRightCircle, goalRightCircle))
            {
                _paths.Add(new RlrDubinsPath(_constraints));
            }
            // 2 x InnerDubinsPath
            if (RslDubinsPath.PathIsPossible(startRightCircle, goalLeftCircle))
            {
                _paths.Add(new RslDubinsPath(_constraints));
            }
            if (LsrDubinsPath.PathIsPossible(startLeftCircle, goalRightCircle))
            {
                _paths.Add(new LsrDubinsPath(_constraints));
            }
            _paths.Sort((x, y) => x.TotalLength.CompareTo(y.TotalLength));
        }

        public ReadOnlyCollection<DubinsPath> Paths => _paths.AsReadOnly();

    }

}
