// Copyright Rising Sun. All Rights Reserved.

#include "Tasks/TaskStopVehicle.h"
#include "MassStateTreeExecutionContext.h"
#include "StateTreeLinker.h"
#include "MassStateTreeDependency.h"
#include "MassTrafficFragments.h"
#include "MassMovementFragments.h"
#include "TagsVehicleAI.h"
#include "VehicleAI.h"

FTaskStopVehicle::FTaskStopVehicle()
{
    bShouldCallTick = true;
    bShouldCallTickOnlyOnEvents = true;
}

bool FTaskStopVehicle::Link(FStateTreeLinker& Linker)
{
    Linker.LinkExternalData(TrafficControlHandle);
    Linker.LinkExternalData(ActorFragmentHandle);
    return true;
}

void FTaskStopVehicle::GetDependencies(UE::MassBehavior::FStateTreeDependencyBuilder& Builder) const
{
    Builder.AddReadWrite<FMassTrafficPIDVehicleControlFragment>();
    Builder.AddReadOnly<FMassActorFragment>();
}

const UStruct* FTaskStopVehicle::GetInstanceDataType() const
{
    return FInstanceDataType::StaticStruct();
}

EStateTreeRunStatus FTaskStopVehicle::EnterState(FStateTreeExecutionContext& Context, const FStateTreeTransitionResult& Transition) const
{
    Super::EnterState(Context, Transition);
    
    FMassStateTreeExecutionContext& MassContext = static_cast<FMassStateTreeExecutionContext&>(Context);
    FInstanceDataType& InstanceData = Context.GetInstanceData(*this);
    
    if (FMassTrafficPIDVehicleControlFragment* TrafficControl = Context.GetExternalDataPtr(TrafficControlHandle))
    {
        TrafficControl->Throttle = 0.0f;
        TrafficControl->Brake = InstanceData.BrakeForce;
        TrafficControl->Steering = 0.0f;
    }

    MassContext.GetEntityManager().Defer().AddTag<FVehicleAIStoppingTag>(MassContext.GetEntity());
    InstanceData.TimeRemaining = InstanceData.WaitDuration;

    return EStateTreeRunStatus::Running;
}

// Вызывается при активации дерева с помощью сигнала от процессора. НЕ КАЖДЫЙ КАДР!
EStateTreeRunStatus FTaskStopVehicle::Tick(FStateTreeExecutionContext& Context, const float DeltaTime) const
{
    FInstanceDataType& InstanceData = Context.GetInstanceData(*this);
    const FMassActorFragment* ActorFragment = Context.GetExternalDataPtr(ActorFragmentHandle);
    FMassTrafficPIDVehicleControlFragment* TrafficControl = Context.GetExternalDataPtr(TrafficControlHandle);
    
    if (TrafficControl)
    {
        TrafficControl->Throttle = 0.0f;
        TrafficControl->Brake = InstanceData.BrakeForce;
        TrafficControl->Steering = 0.0f;
    }

    bool bIsStopped = false;
    const AActor* Actor = ActorFragment ? ActorFragment->Get() : nullptr;
    if (Actor)
    {
        // Получаем скорость напрямую из физического тела, а не из FMassVelocityFragment,
        // который может быть перезаписан стандартными процессорами трафика.
        const float CurrentSpeed = Actor->GetVelocity().Size();
        bIsStopped = CurrentSpeed < InstanceData.StopSpeedThreshold;
    }

    if (bIsStopped)
    {
        InstanceData.TimeRemaining -= DeltaTime;
        if (InstanceData.TimeRemaining <= 0.0f)
        {
            return EStateTreeRunStatus::Succeeded;
        }
    }
    else
    {
        // Если машина еще не остановилась, сбрасываем таймер ожидания
        InstanceData.TimeRemaining = InstanceData.WaitDuration;
    }

    return EStateTreeRunStatus::Running;
}

void FTaskStopVehicle::ExitState(FStateTreeExecutionContext& Context, const FStateTreeTransitionResult& Transition) const
{
    Super::ExitState(Context, Transition);
    FMassStateTreeExecutionContext& MassContext = static_cast<FMassStateTreeExecutionContext&>(Context);
    
    if (FMassTrafficPIDVehicleControlFragment* TrafficControl = Context.GetExternalDataPtr(TrafficControlHandle))
    {
        TrafficControl->Brake = 0.0f;
    }

    MassContext.GetEntityManager().Defer().RemoveTag<FVehicleAIStoppingTag>(MassContext.GetEntity());
}