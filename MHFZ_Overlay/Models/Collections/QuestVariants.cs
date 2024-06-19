// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The quest variants list.
/// </summary>
public static class QuestVariants
{
    /// <summary>
    /// Defaulty for GRank is 8,0,0,0
    /// </summary>
    public static ReadOnlyDictionary<long, QuestsQuestVariant> QuestIDVariant { get; } = new(new Dictionary<long, QuestsQuestVariant>
        {
            {
                Numbers.QuestIDStarvingDeviljhoHistoric20m, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 32, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDRulingGuanzorumu, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 34, QuestVariant3 = 1, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDStarvingDeviljhoArena, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDShiftingMiRu, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 32, QuestVariant3 = 4, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDThirstyPariapuria, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 32, QuestVariant3 = 4, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBlinkingNargacugaForest, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDHowlingZinogreForest, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBurningFreezingElzelionTower, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 5, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDStarvingDeviljhoHistoric, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDHowlingZinogreHistoric, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDShiftingMiRuHistoric, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 32, QuestVariant3 = 4, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBlinkingNargacugaHistoric, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 3, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBurningFreezingElzelionHistoric, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 5, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBombardierBogabadorumu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 5, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDSparklingZerureusu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 34, QuestVariant3 = 5, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBurningFreezingElzelionTower3m, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 42, QuestVariant3 = 13, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDBurningFreezingElzelionTowerUltimate, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 38, QuestVariant3 = 13, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDSecondDistrictDuremudira, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 128, QuestVariant3 = 8, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDArrogantDuremudiraRepel, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 128, QuestVariant3 = 9, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDArrogantDuremudira, new QuestsQuestVariant{QuestVariant1 = 11, QuestVariant2 = 130, QuestVariant3 = 9, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDZ4AkuraVashimu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDZ4Anorupatisu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Baruragaru, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Blangonga, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Bogabadorumu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4DaimyoHermitaur, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Doragyurosu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Espinas, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Gasurabazura, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Giaorugu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Gravios, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Harudomerugu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Hypnocatrice, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Hyujikiki, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Inagami, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Khezu, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Midogaron, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Plesioth, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Rathalos, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Rukodiora, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4TaikunZamuza, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Tigrex, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },
{
                Numbers.QuestIDZ4Toridcless, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 0, QuestVariant3 = 16, QuestVariant4 = 0}
            },

            {
                Numbers.QuestIDLV9999Fatalis, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 1, QuestVariant3 = 0, QuestVariant4 = 0}
            },
                        {
                Numbers.QuestIDLV9999CrimsonFatalis, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 1, QuestVariant3 = 0, QuestVariant4 = 0}
            },
                                    {
                Numbers.QuestIDLV9999Disufiroa, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 1, QuestVariant3 = 0, QuestVariant4 = 0}
            },
                                                {
                Numbers.QuestIDLV9999Shantien, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 1, QuestVariant3 = 0, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDLowerShitenUnknown, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 64, QuestVariant3 = 0, QuestVariant4 = 0}
            },
                        {
                Numbers.QuestIDLowerShitenDisufiroa, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 64, QuestVariant3 = 0, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDUpperShitenDisufiroa, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 64, QuestVariant3 = 0, QuestVariant4 = 0}
            },
                        {
                Numbers.QuestIDUpperShitenUnknown, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 64, QuestVariant3 = 0, QuestVariant4 = 0}
            },
            {
                Numbers.QuestIDKeoaruboru, new QuestsQuestVariant{QuestVariant1 = 8, QuestVariant2 = 32, QuestVariant3 = 32, QuestVariant4 = 0}
            },
        });
}
